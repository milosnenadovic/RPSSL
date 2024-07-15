using RPSSL.GameService.Domain.Enums;

namespace RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;

public record GetScoreboardQueryResponse
{
	public short No { get; set; }
	public short PlayerChoiceId { get; set; }
	public short ComputerChoiceId { get; set; }
	public DateTime PlayedAt { get; set; }
	public GameResult Result { get; set; }
}