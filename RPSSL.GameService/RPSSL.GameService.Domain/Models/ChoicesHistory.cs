using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class ChoicesHistory : BaseAuditableEntity
{
	public required string PlayerId { get; set; }
	public short PlayerChoiceId { get; set; }
	public short ComputerChoiceId { get; set; }
}
