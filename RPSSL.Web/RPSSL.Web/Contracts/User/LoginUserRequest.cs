using System.ComponentModel.DataAnnotations;

namespace RPSSL.Web.Contracts.User;

public record LoginUserRequest
{
    public string? Username { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool PersistentLogin { get; set; }
}
