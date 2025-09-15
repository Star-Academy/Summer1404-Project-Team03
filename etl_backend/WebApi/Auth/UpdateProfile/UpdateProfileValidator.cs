using FastEndpoints;
using FluentValidation;

namespace WebApi.Auth.UpdateProfile;

public class UpdateProfileValidator : Validator<UpdateProfileRequest>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.FirstName) ||
                       !string.IsNullOrWhiteSpace(x.LastName) ||
                       !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("At least one field (FirstName, LastName, Email) must be provided.");
    }
}