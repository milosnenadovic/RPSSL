using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class Choice : BaseEntity
{
	public required string Name { get; set; }
	public required string Image { get; set; }

	public virtual ICollection<ChoicesHistory> ChoiceWins { get; set; } = [];
}
