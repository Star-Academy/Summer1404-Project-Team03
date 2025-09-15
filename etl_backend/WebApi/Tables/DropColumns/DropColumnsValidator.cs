using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.DropColumns;

public class DropColumnsValidator : Validator<DropColumnsRequest>
{
    public DropColumnsValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
        RuleFor(x => x.ColumnIds).NotEmpty();
    }
}