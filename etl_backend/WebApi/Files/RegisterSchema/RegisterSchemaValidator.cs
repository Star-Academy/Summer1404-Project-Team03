using FastEndpoints;
using FluentValidation;

namespace WebApi.Files;

public class RegisterSchemaValidator : Validator<RegisterSchemaRequest>
{
    public RegisterSchemaValidator()
    {
        // RuleFor(x => x.Id)
        //     .GreaterThan(0)
        //     .WithMessage("Staged file ID must be greater than 0.");

        // RuleFor(x => x.Columns)
        //     .NotEmpty()
        //     .WithMessage("At least one column is required.");

        RuleForEach(x => x.Columns)
            .Must(c => c.OrdinalPosition >= 0)
            .WithMessage("OrdinalPosition must be > 0.");

        RuleForEach(x => x.Columns)
            .Must(c => !string.IsNullOrWhiteSpace(c.ColumnType))
            .WithMessage("ColumnType is required.");
    }
}