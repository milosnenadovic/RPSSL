using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Common.Services;

public class CurrentUser
{
	public required string Email { get; set; }
	public required string Name { get; set; }
	public required string UserId { get; set; }
	public Role Role { get; set; }
	public int OrganizationId { get; set; }
	public int UserOrganizationId { get; set; }
	public bool CanAddDocument { get; set; }
	public bool CanArchiveDocument { get; set; }
	public bool CanSeeAllDocuments { get; set; }
	public required string AuthToken { get; set; }
}