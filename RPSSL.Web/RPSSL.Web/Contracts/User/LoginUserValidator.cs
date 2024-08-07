﻿using FluentValidation;
using RPSSL.Web.Helpers;

namespace RPSSL.Web.Contracts.User;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(LocalizationManager.StaticGet("RequiredField"))
            .MaximumLength(50).WithMessage(LocalizationManager.StaticGet("NotLongerThan50Chars"))
            .MinimumLength(8).WithMessage(LocalizationManager.StaticGet("NotLessThan8Chars"))
            .Must(BeValidPasswordFormat).WithMessage(LocalizationManager.StaticGet("InvalidData"));

        RuleFor(p => p.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(LocalizationManager.StaticGet("RequiredField"))
            .MaximumLength(24).WithMessage(LocalizationManager.StaticGet("NotLongerThan24Chars"));
    }

    private bool BeValidPasswordFormat(string password) => PasswordValidator.IsValidPassword(password);
}
