using RPSSL.GameService.Domain.Models.Base;
using System.ComponentModel;

namespace RPSSL.GameService.Domain.Models;

public class User : BaseAuditableIdentityEntity
{
	public Role Role { get; set; } = Role.User;

	public virtual ICollection<ChoicesHistory>? ChoicesHistory { get; set; }
}

public enum Role
{
	[Description("User")]
	User = 1,
	[Description("Admin")]
	Admin = 2
}