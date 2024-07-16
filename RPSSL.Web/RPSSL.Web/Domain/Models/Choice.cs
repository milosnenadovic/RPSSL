using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class Choice
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("choiceWins")]
    public virtual ICollection<ChoiceWin>? ChoiceWins { get; set; }
}
