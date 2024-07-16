using RPSSL.Web.Domain.Enums;
using System.Text.Json.Serialization;

namespace RPSSL.Web.Domain.Models;

public class Scoreboard
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("playerChoiceId")]
    public short PlayerChoiceId { get; set; }
    [JsonPropertyName("computerChoiceId")]
    public short ComputerChoiceId { get; set; }
    [JsonPropertyName("playedAt")]
    public DateTime PlayedAt { get; set; }
    [JsonPropertyName("result")]
    public GameResult Result { get; set; }
}
