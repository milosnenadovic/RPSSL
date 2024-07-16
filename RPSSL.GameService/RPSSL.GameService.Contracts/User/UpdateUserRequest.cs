namespace RPSSL.GameService.Contracts.User;

public record UpdateUserRequest
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public bool Active { get; set; }
}
