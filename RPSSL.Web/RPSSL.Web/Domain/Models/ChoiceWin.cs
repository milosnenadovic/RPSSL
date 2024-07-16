using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class ChoiceWin
{
    [JsonPropertyName("beatsChoiceId")]
    public int BeatsChoiceId { get; set; }
    [JsonPropertyName("actionName")]
    public required string ActionName { get; set; }
}
