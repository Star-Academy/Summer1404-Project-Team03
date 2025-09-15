using FastEndpoints;
using FluentValidation;

namespace WebApi.Workflow;

public class CreateWorkflowValidator : Validator<CreateWorkflowRequest>
{
    public CreateWorkflowValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(256);
    }
}