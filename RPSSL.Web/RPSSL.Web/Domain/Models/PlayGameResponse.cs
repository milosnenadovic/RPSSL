using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class PlayGameResponse
{
    [JsonPropertyName("results")]
    public required string Results { get; set; }
    [JsonPropertyName("player")]
    public short Player { get; set; }
    [JsonPropertyName("computer")]
    public short Computer { get; set; }
}
