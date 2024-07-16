using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class Language
{
    [JsonPropertyName("id")]
    public short Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("name")]
    public required string LanguageCode { get; set; }
    [JsonPropertyName("countryId")]
    public required string CountryId { get; set; }
}
