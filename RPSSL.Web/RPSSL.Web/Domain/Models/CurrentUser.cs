using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class CurrentUser : User
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("authToken")]
    public required string AuthToken { get; set; }
}
