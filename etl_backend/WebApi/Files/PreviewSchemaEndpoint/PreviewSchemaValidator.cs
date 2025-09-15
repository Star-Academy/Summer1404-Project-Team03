using FastEndpoints;
using FluentValidation;

namespace WebApi.Files;

public class PreviewSchemaValidator : Validator<PreviewSchemaRequest>
{
    public PreviewSchemaValidator()
    {
        // RuleFor(x => x.Id)
        //     .GreaterThan(0)
        //     .WithMessage("Staged file ID must be greater than 0.");
    }
}