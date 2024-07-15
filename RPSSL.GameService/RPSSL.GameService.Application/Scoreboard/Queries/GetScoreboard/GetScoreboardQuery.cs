using MediatR;
using RPSSL.GameService.Application._Common.DTO;
using RPSSL.GameService.Common.Response;
using RPSSL.GameService.Contracts._Common;
using RPSSL.GameService.Contracts._Common.SortBy;
using RPSSL.GameService.Domain.Enums;

namespace RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;

public record GetScoreboardQuery(short? PlayerChoiceId, short? ComputerChoiceId, GameResult? Result, DateTime? PlayedFrom, DateTime? PlayedTo)
	: BaseQueryStringParameters<GetScoreboardSortBy>, IRequest<IResponse<PaginatedList<GetScoreboardQueryResponse>>>;
