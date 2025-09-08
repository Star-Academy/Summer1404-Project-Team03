using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.RenameTable;

public class RenameTableValidator : Validator<RenameTableRequest>
{
    public RenameTableValidator()
    {
        RuleFor(x => x.NewTableName)
            .NotEmpty()
            .WithMessage("NewTableName is required.");
    }
}