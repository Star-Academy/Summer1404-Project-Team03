using FastEndpoints;
using FluentValidation;

namespace WebApi.Users.CreateUser;

public class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.");
    }
}