using System.Text.Json.Serialization;

namespace RPSSL.GameService.Infrastructure.Services.RandomGeneratedNumber;

public record RandomGeneratedNumberResponse
{
    [JsonPropertyName("random_number")]
    public short RandomNumber { get; set; }
}
