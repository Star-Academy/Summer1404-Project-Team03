using FastEndpoints;
using FluentValidation;

namespace WebApi.Tables.GetTableDetails;

public class GetTableDetailsValidator : Validator<GetTableDetailsRequest>
{
    public GetTableDetailsValidator()
    {
        RuleFor(x => x.SchemaId).GreaterThan(0);
    }
}