using System.Text.Json.Serialization;

namespace RPSSL.Web.Contracts.Choice;

public record PlayGameRequest
{
    [JsonPropertyName("player")]
    public short PlayerChoiceId { get; set; }
}
