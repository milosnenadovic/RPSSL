using Microsoft.AspNetCore.Identity;

namespace RPSSL.GameService.Domain.Models.Base;

public class BaseAuditableIdentityEntity : IdentityUser
{
	public DateTime Created { get; set; }
	public string CreatedBy { get; set; } = null!;
	public DateTime? LastModified { get; set; }
	public string? LastModifiedBy { get; set; }
	public bool Active { get; set; }
}
