using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.PreviewTableRows;

public class PreviewTableRowsValidator : Validator<PreviewTableRowsRequest>
{
    public PreviewTableRowsValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
        RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(200);
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}