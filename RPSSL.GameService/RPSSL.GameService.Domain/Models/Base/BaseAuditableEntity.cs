namespace RPSSL.GameService.Domain.Models.Base;

public abstract class BaseAuditableEntity : BaseEntity
{
	public DateTime Created { get; set; }
	public required string CreatedBy { get; set; }
	public DateTime? LastModified { get; set; }
	public string? LastModifiedBy { get; set; }
	public bool Active { get; set; }
}