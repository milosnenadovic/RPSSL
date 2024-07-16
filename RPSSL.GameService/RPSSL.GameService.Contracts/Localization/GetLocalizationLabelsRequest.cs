namespace RPSSL.GameService.Contracts.Localization;

public record GetLocalizationLabelsRequest
{
    public short LanguageId { get; set; }
}
