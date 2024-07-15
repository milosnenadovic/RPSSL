namespace RPSSL.GameService.Contracts.User;

public record LoginUserRequest
{
	public required string Username { get; set; }
	public required string Password { get; set; }
	public bool PersistentLogin { get; set; }
}
