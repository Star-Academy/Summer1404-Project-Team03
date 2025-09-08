using FastEndpoints;
using FluentValidation;

namespace WebApi.Files;

public class DeleteStagedFileValidator : Validator<DeleteStagedFileRequest>
{
    public DeleteStagedFileValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Staged file ID must be greater than 0.");
    }
}