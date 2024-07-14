using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Common.Services;

public class CurrentUser
{
	public required string Email { get; set; }
	public required string Username { get; set; }
	public required string UserId { get; set; }
	public Role Role { get; set; }
	public required string AuthToken { get; set; }
}