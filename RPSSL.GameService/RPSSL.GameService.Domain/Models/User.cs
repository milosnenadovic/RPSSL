using RPSSL.GameService.Domain.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPSSL.GameService.Domain.Models;

public class User : BaseAuditableIdentityEntity
{
	public Role Role { get; set; } = Role.User;

	[NotMapped]
	public string? AuthToken { get; set; }
}

public enum Role
{
	[Description("User")]
	User = 1,
	[Description("Admin")]
	Admin = 2
}