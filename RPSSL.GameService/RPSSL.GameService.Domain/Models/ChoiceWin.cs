using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class ChoiceWin : BaseEntity
{
	public int ChoiceId { get; set; }
	public int BeatsChoiceId { get; set; }
	public required string ActionName { get; set; }
}
