using RPSSL.GameService.Domain.Models.Base;

namespace RPSSL.GameService.Domain.Models;

public class User : BaseAuditableIdentityEntity
{
	public virtual ICollection<ChoicesHistory>? ChoicesHistory { get; set; }
}
