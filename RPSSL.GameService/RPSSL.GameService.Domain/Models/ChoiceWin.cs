using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class ChoiceWin : BaseEntity
{
	public short ChoiceId { get; set; }
	public short BeatsChoiceId { get; set; }
	public required string ActionName { get; set; }
}
