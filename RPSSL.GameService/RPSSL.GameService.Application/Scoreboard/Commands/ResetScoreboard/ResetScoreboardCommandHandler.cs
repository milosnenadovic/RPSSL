using MediatR;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Common.Services;
using RPSSL.GameService.Domain.Abstractions;

namespace RPSSL.GameService.Application.Scoreboard.Commands.ResetScoreboard;

public class ResetScoreboardCommandHandler(IChoicesHistoryRepository choicesHistoryRepository,
	ICurrentUserService currentUserService, IUserRepository userRepository)
	: IRequestHandler<ResetScoreboardCommand, IResponse<bool>>
{
	private readonly IChoicesHistoryRepository _choicesHistoryRepository = choicesHistoryRepository;
	private readonly IUserRepository _userRepository = userRepository;
	private readonly ICurrentUserService _currentUserService = currentUserService;

	public async Task<IResponse<bool>> Handle(ResetScoreboardCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetById(_currentUserService.CurrentUser.UserId);
		if (user is null)
			return new ErrorResponse<bool>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.User);

		await _choicesHistoryRepository.DeleteAll(user.Id, cancellationToken);

		return new SuccessResponse<bool>(true);
	}
}
