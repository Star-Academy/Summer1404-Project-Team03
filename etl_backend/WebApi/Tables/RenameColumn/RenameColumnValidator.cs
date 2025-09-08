using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.RenameColumn;

public class RenameColumnValidator : Validator<RenameColumnRequest>
{
    public RenameColumnValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
        RuleFor(x => x.ColumnId).GreaterThan(0);
        RuleFor(x => x.NewName).NotEmpty();
    }
}