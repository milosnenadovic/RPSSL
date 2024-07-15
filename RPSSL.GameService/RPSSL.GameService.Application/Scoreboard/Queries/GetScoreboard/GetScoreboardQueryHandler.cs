using Mapster;
using MediatR;
using RPSSL.GameService.Application._Common.DTO;
using RPSSL.GameService.Common.Constants.Errors;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Common.Services;
using RPSSL.GameService.Domain.Abstractions;
using RPSSL.GameService.Domain.Enums;
using RPSSL.GameService.Domain.Filters;

namespace RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;

public class GetScoreboardQueryHandler(IChoicesHistoryRepository choicesHistoryRepository, IChoiceWinRepository choiceWinRepository,
	IUserRepository userRepository, ICurrentUserService currentUserService)
	: IRequestHandler<GetScoreboardQuery, IResponse<PaginatedList<GetScoreboardQueryResponse>>>
{
	private readonly IChoicesHistoryRepository _choicesHistoryRepository = choicesHistoryRepository;
	private readonly IChoiceWinRepository _choiceWinRepository = choiceWinRepository;
	private readonly IUserRepository _userRepository = userRepository;
	private readonly ICurrentUserService _currentUserService = currentUserService;

	public async Task<IResponse<PaginatedList<GetScoreboardQueryResponse>>> Handle(GetScoreboardQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetById(_currentUserService.CurrentUser.UserId);
		if (user is null)
			return new ErrorResponse<PaginatedList<GetScoreboardQueryResponse>>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.User);

		var choicesHistory = await _choicesHistoryRepository.GetChoicesHistory(
			new GetChoicesHistoryFilter(
				user.Id,
				request.PlayerChoiceId,
				request.ComputerChoiceId,
				request.Result,
				request.PlayedFrom,
				request.PlayedTo,
				true)
			{
				PageNumber = request.PageNumber,
				PageSize = request.PageSize,
				SortBy = (int)request.SortBy,
				SortDescending = request.SortDescending
			}, cancellationToken);

		if (choicesHistory.Key is null)
			return new ErrorResponse<PaginatedList<GetScoreboardQueryResponse>>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.Choice);

		var results = choicesHistory.Key.Adapt<List<GetScoreboardQueryResponse>>();

		var wins = await _choiceWinRepository.GetChoiceWins(null, cancellationToken);
		if (wins is null)
			return new ErrorResponse<PaginatedList<GetScoreboardQueryResponse>>((int)ErrorCodes.DatabaseGet, ErrorCodes.DatabaseGet.ToString(), Error.DatabaseGet.ChoiceWins);

		foreach (var result in results)
		{
			if (result.PlayerChoiceId == result.ComputerChoiceId)
				result.Result = GameResult.Tie;
			else
			{
				if (wins.Any(x => x.ChoiceId == result.PlayerChoiceId && x.BeatsChoiceId == result.ComputerChoiceId))
					result.Result = GameResult.Win;
				else
					result.Result = GameResult.Lose;
			}
		}

		var paginatedList = new PaginatedList<GetScoreboardQueryResponse>(results, choicesHistory.Value, request.PageNumber, request.PageSize);

		return new SuccessResponse<PaginatedList<GetScoreboardQueryResponse>>(paginatedList);
	}
}
