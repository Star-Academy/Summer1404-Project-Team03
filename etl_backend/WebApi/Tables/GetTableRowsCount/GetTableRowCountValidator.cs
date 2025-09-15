using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.GetTableRowsCount;

public class GetTableRowCountValidator : Validator<GetTableRowCountRequest>
{
    public GetTableRowCountValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
    }
}