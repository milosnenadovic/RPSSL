using System.ComponentModel.DataAnnotations;

namespace RPSSL.Web.Contracts.User;

public record RegisterUserRequest
{
    [Required]
    public required string Username { get; set; }
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    public string? Phone { get; set; }
}
