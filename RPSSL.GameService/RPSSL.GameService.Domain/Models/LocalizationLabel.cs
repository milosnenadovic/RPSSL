namespace RPSSL.GameService.Domain.Models;

public class LocalizationLabel
{
    public required string Key { get; set; }
    public required int LanguageId { get; set; }
    public required string Value { get; set; }

    public virtual Language Language { get; set; } = null!;
}
