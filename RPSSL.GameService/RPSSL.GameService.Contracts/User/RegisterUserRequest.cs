namespace RPSSL.GameService.Contracts.User;

public record RegisterUserRequest
{
	public required string Email { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
	public string? Phone { get; set; }
}
