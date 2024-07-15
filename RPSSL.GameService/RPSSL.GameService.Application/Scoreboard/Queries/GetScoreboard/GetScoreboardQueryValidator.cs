using FluentValidation;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Scoreboard.Queries.GetScoreboard;

public class GetScoreboardQueryValidator : AbstractValidator<GetScoreboardQuery>
{
	public GetScoreboardQueryValidator()
	{
		RuleFor(v => v.PageNumber)
			.NotNull().WithMessage(Error.Required.PageNumber)
			.GreaterThan(0).WithMessage(Error.InvalidData.PageNumber);

		RuleFor(v => v.PageSize)
			.NotNull().WithMessage(Error.Required.PageSize)
			.GreaterThan(0).WithMessage(Error.InvalidData.PageSize);

		RuleFor(v => v.PlayerChoiceId)
			.InclusiveBetween((short)1, (short)5).WithMessage(Error.InvalidData.PlayerChoice)
			.When(v => v.PlayerChoiceId.HasValue);

		RuleFor(v => v.ComputerChoiceId)
			.InclusiveBetween((short)1, (short)5).WithMessage(Error.InvalidData.ComputerChoice)
			.When(v => v.ComputerChoiceId.HasValue);
	}
}