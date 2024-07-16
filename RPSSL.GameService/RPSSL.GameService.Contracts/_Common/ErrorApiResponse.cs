using System.Text.Json.Serialization;

namespace RPSSL.GameService.Contracts._Common;

public record ErrorApiResponse
{
    [JsonPropertyName("title")]
    [JsonPropertyOrder(1)]
    public string Title { get; set; } = null!;
    [JsonPropertyName("detail")]
    [JsonPropertyOrder(2)]
    public string Detail { get; set; } = null!;
    [JsonPropertyName("errorCode")]
    [JsonPropertyOrder(3)]
    public int? ErrorCode { get; set; } = null!;
    [JsonPropertyName("errorCodes")]
    [JsonPropertyOrder(4)]
    public List<ErrorCode> ErrorCodes { get; set; } = null!;
}

public record ErrorCode
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
}
