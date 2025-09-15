using FastEndpoints;
using FluentValidation;

namespace WebApi.Auth.Token;

public class TokenValidator : Validator<TokenRequest>
{
    public TokenValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Authorization code is required.");

        RuleFor(x => x.RedirectUrl)
            .NotEmpty()
            .WithMessage("RedirectUrl is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("RedirectUrl must be a valid absolute URL.");
    }
}