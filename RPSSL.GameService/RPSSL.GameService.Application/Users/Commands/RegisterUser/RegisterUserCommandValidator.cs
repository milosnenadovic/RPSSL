using FluentValidation;
using RPSSL.GameService.Application._Common.Helpers;
using RPSSL.GameService.Common.Constants.Errors;

namespace RPSSL.GameService.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty().WithMessage(Error.Required.Name)
            .MaximumLength(24).WithMessage(Error.InvalidData.UserNameLongerThan24);

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage(Error.Required.Email)
            .EmailAddress().WithMessage(Error.InvalidData.EmailFormat)
            .MaximumLength(96).WithMessage(Error.InvalidData.EmailLongerThan96);

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage(Error.Required.Password)
            .MaximumLength(50).WithMessage(Error.InvalidData.PasswordLongerThan50)
            .MinimumLength(8).WithMessage(Error.InvalidData.PasswordShorterThan8)
            .Must(BeValidPasswordFormat).WithMessage(Error.InvalidData.PasswordFormat);

        RuleFor(v => v.Phone)
            .MaximumLength(48).WithMessage(Error.InvalidData.PhoneLongerThan48);
    }

    private bool BeValidPasswordFormat(string password) => PasswordValidator.IsValidPassword(password);
}
