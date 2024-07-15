using MediatR;
using Microsoft.Extensions.Logging;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Models;
using System.Transactions;

namespace RPSSL.GameService.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userService, IUnitOfWork unitOfWork, ILogger<RegisterUserCommandHandler> logger)
	: IRequestHandler<RegisterUserCommand, IResponse<bool>>
{
	private readonly IUserRepository _userService = userService;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ILogger<RegisterUserCommandHandler> _logger = logger;

	public async Task<IResponse<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var existingUser = await _userService.GetByUserName(request.Username);
		if (existingUser is not null)
			return new ErrorResponse<bool>((int)ErrorCodes.AlreadyExists, ErrorCodes.AlreadyExists.ToString(), Error.AlreadyExists.User);
		existingUser = await _userService.GetByEmail(request.Email);
		if (existingUser is not null)
			return new ErrorResponse<bool>((int)ErrorCodes.AlreadyExists, ErrorCodes.AlreadyExists.ToString(), Error.AlreadyExists.User);

		try
		{
			using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

			var newUser = new User
			{
				UserName = request.Username,
				Email = request.Email,
				PhoneNumber = request.Phone,
				CreatedBy = "system",
				Active = true,
				EmailConfirmed = true,
				PhoneNumberConfirmed = true
			};

			var savedUser = await _userService.Add(newUser, request.Password);
			if (savedUser is null)
				return new ErrorResponse<bool>((int)ErrorCodes.DatabaseAdd, ErrorCodes.DatabaseAdd.ToString(), Error.DatabaseAdd.User);

			savedUser.LastModifiedBy = "system";

			var roleAdded = await _userService.AddToRole(savedUser);
			if (!roleAdded)
				return new ErrorResponse<bool>((int)ErrorCodes.DatabaseAdd, ErrorCodes.DatabaseAdd.ToString(), Error.DatabaseAdd.UserRole);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			transactionScope.Complete();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			return new ErrorResponse<bool>((int)ErrorCodes.UnspecifiedError, ErrorCodes.UnspecifiedError.ToString(), ex.Message);
		}

		return new SuccessResponse<bool>(true);
	}
}
