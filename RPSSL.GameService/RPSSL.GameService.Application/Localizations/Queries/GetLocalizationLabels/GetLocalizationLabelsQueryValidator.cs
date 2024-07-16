using FluentValidation;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Localizations.Queries.GetLocalizationLabels;

public class GetLocalizationLabelsQueryValidator : AbstractValidator<GetLocalizationLabelsQuery>
{
    public GetLocalizationLabelsQueryValidator()
    {
        RuleFor(v => v.LanguageId)
            .NotNull().WithMessage(Error.Required.Language)
            .InclusiveBetween((short)1, (short)5).WithMessage(Error.InvalidData.Language);
    }
}