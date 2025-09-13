using Application.Enums;
using Application.Files.Commands;
using FastEndpoints;
using FluentValidation;

namespace WebApi.Files.RegisterAndLoad;

public class RegisterAndLoadValidator : Validator<RegisterAndLoadRequest>
{
    public RegisterAndLoadValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Staged file ID must be greater than 0.");

        RuleForEach(x => x.Columns)
            .Must(c => c.OrdinalPosition >= 0)
            .WithMessage("OrdinalPosition must be > 0.");

        RuleForEach(x => x.Columns)
            .Must(c => !string.IsNullOrWhiteSpace(c.ColumnType))
            .WithMessage("ColumnType is required.");

        RuleFor(x => x.LoadMode)
            .Must(m => Enum.TryParse<LoadMode>(m, true, out _))
            .WithMessage("LoadMode must be 'Append' or 'Truncate'.");
    }
}