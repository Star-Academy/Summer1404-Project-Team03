using FastEndpoints;
using FluentValidation;

namespace WebApi.Files;

public class StageManyFilesValidator : Validator<StageManyFilesRequest>
{
    public StageManyFilesValidator()
    {
        RuleFor(x => x.Files)
            .NotNull()
            .WithMessage("Files collection is required.");

        RuleForEach(x => x.Files!)
            .Must(f => f != null && f.Length > 0)
            .When(x => x.Files != null)
            .WithMessage("Each file must be non-null and non-empty.");
    }
}