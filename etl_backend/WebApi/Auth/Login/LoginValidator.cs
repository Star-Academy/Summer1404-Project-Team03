using FastEndpoints;
using FluentValidation;

namespace WebApi.Auth.Login;

public class LoginValidator : Validator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.RedirectUrl)
            .NotEmpty()
            .WithMessage("RedirectUrl is required.");
        // .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
        // .WithMessage("RedirectUrl must be a valid absolute URL.");
    }
}