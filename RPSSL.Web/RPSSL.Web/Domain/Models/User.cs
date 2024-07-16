using RPSSL.Web.Domain.Enums;
using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class User
{
    [JsonPropertyName("role")]
    public Role Role { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}
