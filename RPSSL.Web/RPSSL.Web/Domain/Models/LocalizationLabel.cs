using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class LocalizationLabel
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }
    [JsonPropertyName("value")]
    public required string Value { get; set; }
}
