using MediatR;
using Microsoft.Extensions.Logging;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Common.Services;
using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Application.Users.Commands.LogoutUser;

public class LogoutUserCommandHandler(ICurrentUserService currentUserService, IUserRepository userService,
	IUnitOfWork unitOfWork, ILogger<LogoutUserCommandHandler> logger) : IRequestHandler<LogoutUserCommand, IResponse<bool>>
{
	private readonly ICurrentUserService _currentUserService = currentUserService;
	private readonly IUserRepository _userService = userService;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ILogger<LogoutUserCommandHandler> _logger = logger;

	public async Task<IResponse<bool>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
	{
		var existingUser = await _userService.GetById(_currentUserService.CurrentUser.UserId);
		if (existingUser is null)
			return new ErrorResponse<bool>((int)ErrorCodes.CantFind, ErrorCodes.CantFind.ToString(), Error.CantFind.User);

		try
		{
			await _userService.Logout();

			await _unitOfWork.SaveChangesAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			return new ErrorResponse<bool>((int)ErrorCodes.CantFind, ErrorCodes.CantFind.ToString(), ex.Message);
		}

		return new SuccessResponse<bool>(true);
	}
}
