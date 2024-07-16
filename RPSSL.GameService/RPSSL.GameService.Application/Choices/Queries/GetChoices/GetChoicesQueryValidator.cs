using FluentValidation;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Choices.Queries.GetChoices;

public class GetChoicesQueryValidator : AbstractValidator<GetChoicesQuery>
{
    public GetChoicesQueryValidator()
    {
        RuleFor(v => v.FilterName)
            .MaximumLength(12).WithMessage(Error.InvalidData.NameLongerThan12)
            .When(v => !string.IsNullOrEmpty(v.FilterName));
    }
}