using RPSSL.GameService.Contracts._Common;
using RPSSL.GameService.Contracts._Common.SortBy;

namespace RPSSL.GameService.Contracts.Scoreboard;

public record GetScoreboardRequest : BaseQueryStringParameters<GetScoreboardSortBy>
{
	public short? PlayerChoiceId { get; set; }
	public short? ComputerChoiceId { get; set; }
	public int? Result { get; set; }
	public DateTime? PlayedFrom { get; set; }
	public DateTime? PlayedTo { get; set; }
	public bool? Active { get; set; }
}
