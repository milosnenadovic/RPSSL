using System.Text.Json.Serialization;

namespace RPSSL.GameService.Contracts.Choice;

public record PlayGameRequest
{
	[JsonPropertyName("player")]
	public short PlayerChoiceId { get; set; }
}
