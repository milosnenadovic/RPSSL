namespace RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;

public record GetLocalizationLabelsQueryResponse
{
    public short LanguageId { get; set; }
    public List<LocalizationLabelDto> LocalizationLabels { get; set; } = [];
}

public record LocalizationLabelDto
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}
