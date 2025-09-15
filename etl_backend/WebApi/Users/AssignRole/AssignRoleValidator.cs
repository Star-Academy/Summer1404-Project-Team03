using FastEndpoints;
using FluentValidation;

namespace WebApi.Users.AssignRole;

public class AssignRoleValidator : Validator<AssignRoleRequest>
{
    public AssignRoleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("RoleName is required.");
    }
}