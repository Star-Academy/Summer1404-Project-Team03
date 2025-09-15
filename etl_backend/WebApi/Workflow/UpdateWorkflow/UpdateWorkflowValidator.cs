using FastEndpoints;
using FluentValidation;

namespace WebApi.Workflow.UpdateWorkflow;

public class UpdateWorkflowValidator : Validator<UpdateWorkflowRequest>
{
    public UpdateWorkflowValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(256);

        RuleFor(x => x.Status)
            .Must(s => string.IsNullOrWhiteSpace(s) ||
                       new[] { "Draft", "Running", "Completed", "Failed" }.Contains(s))
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be Draft, Running, Completed, or Failed.");
    }
}