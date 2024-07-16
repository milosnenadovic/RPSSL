using FluentValidation;
using RPSSL.GameService.Application._Common.Helpers;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage(Error.Required.Name)
            .MaximumLength(24).WithMessage(Error.InvalidData.UserNameLongerThan24);

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage(Error.Required.Password)
            .MaximumLength(50).WithMessage(Error.InvalidData.PasswordLongerThan50)
            .MinimumLength(8).WithMessage(Error.InvalidData.PasswordShorterThan8)
            .Must(BeValidPasswordFormat).WithMessage(Error.InvalidData.PasswordFormat);
    }

    private bool BeValidPasswordFormat(string password) => PasswordValidator.IsValidPassword(password);
}