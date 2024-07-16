using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class LocalizedData
{
    [JsonPropertyName("languageId")]
    public short LanguageId { get; set; }
    [JsonPropertyName("localizationLabels")]
    public List<LocalizationLabel> LocalizationLabels { get; set; } = [];
}
