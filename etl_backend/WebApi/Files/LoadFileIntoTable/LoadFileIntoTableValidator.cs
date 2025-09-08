using Application.Files.Commands;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Files;

public class LoadFileIntoTableValidator : Validator<LoadFileIntoTableRequest>
{
    public LoadFileIntoTableValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Staged file ID must be greater than 0.");

        RuleFor(x => x.Mode)
            .Must(m => Enum.TryParse<LoadMode>(m, true, out _))
            .WithMessage("Mode must be 'Append' or 'Truncate'.");
    }
}