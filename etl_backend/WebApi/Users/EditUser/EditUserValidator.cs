using FastEndpoints;
using FluentValidation;

namespace WebApi.Users.EditUser;

public class EditUserValidator : Validator<EditUserRequest>
{
    public EditUserValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Username) ||
                       !string.IsNullOrWhiteSpace(x.Email) ||
                       !string.IsNullOrWhiteSpace(x.FirstName) ||
                       !string.IsNullOrWhiteSpace(x.LastName))
            .WithMessage("At least one field (Username, Email, FirstName, LastName) must be provided.");
    }
}