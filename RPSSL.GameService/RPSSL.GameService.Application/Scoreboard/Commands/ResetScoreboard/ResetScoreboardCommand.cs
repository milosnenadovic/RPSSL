using MediatR;
using RPSSL.GameService.Common.Response;

namespace RPSSL.GameService.Application.Scoreboard.Commands.ResetScoreboard;

public record ResetScoreboardCommand : IRequest<IResponse<bool>>;
