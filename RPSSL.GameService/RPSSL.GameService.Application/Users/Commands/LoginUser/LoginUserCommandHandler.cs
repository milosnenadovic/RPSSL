using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RPSSL.GameService.Common.Configurations.Settings;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Helpers.Auth;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IUserRepository userService, IUnitOfWork unitOfWork, ILogger<LoginUserCommandHandler> logger, IOptions<JwtSettings> jwtSettings)
    : IRequestHandler<LoginUserCommand, IResponse<LoginUserCommandResponse>>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly IUserRepository _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<LoginUserCommandHandler> _logger = logger;

    public async Task<IResponse<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userService.GetByUserName(request.Username);
        if (existingUser is null)
            return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.CantFind, ErrorCodes.CantFind.ToString(), Error.CantFind.User);
        if (!existingUser.Active)
            return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.Authorization, ErrorCodes.Authorization.ToString(), Error.Authorization.UserInactive);

        var validPassword = await _userService.CheckPassword(existingUser, request.Password);

        if (!validPassword)
            return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.Authorization, ErrorCodes.Authorization.ToString(), Error.Authorization.Login);
        if (!existingUser.EmailConfirmed)
            return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.Authorization, ErrorCodes.Authorization.ToString(), Error.Authorization.UnconfirmedEmail);

        try
        {
            var loggedInUser = await _userService.Login(existingUser, request.Password, request.PersistentLogin);
            if (loggedInUser is null)
                return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.Authorization, ErrorCodes.Authorization.ToString(), Error.Authorization.Login);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            existingUser.AuthToken = AuthTokenHelper.GenerateAuthToken(
                existingUser.Email ?? string.Empty,
                existingUser.Id,
                existingUser.UserName ?? string.Empty,
                existingUser.Role,
                _jwtSettings.SecretKey,
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                _jwtSettings.ExpiryMinutes
            );
            var user = existingUser.Adapt<LoginUserCommandResponse>();

            return new SuccessResponse<LoginUserCommandResponse>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ErrorResponse<LoginUserCommandResponse>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), ex.Message);
        }
    }
}
