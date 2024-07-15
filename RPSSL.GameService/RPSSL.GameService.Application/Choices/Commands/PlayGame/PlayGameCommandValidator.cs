using FluentValidation;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Choices.Commands.PlayGame;

public class PlayGameCommandValidator : AbstractValidator<PlayGameCommand>
{
	public PlayGameCommandValidator()
	{
		RuleFor(v => v.PlayerChoiceId)
			.NotNull().WithMessage(Error.Required.PageNumber)
			.InclusiveBetween((short)1, (short)5).WithMessage(Error.InvalidData.PlayerChoice);
	}
}