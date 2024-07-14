namespace RPSSL.GameService.Contracts.User;

public record LoginUserRequest
{
	public required string UserName { get; set; }
	public required string Password { get; set; }
}
