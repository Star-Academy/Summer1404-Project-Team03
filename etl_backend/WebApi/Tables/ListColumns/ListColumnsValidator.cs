using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.ListColumns;

public class ListColumnsValidator : Validator<ListColumnsRequest>
{
    public ListColumnsValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
    }
}