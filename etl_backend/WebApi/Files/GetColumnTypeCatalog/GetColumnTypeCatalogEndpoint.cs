using Application.Abstractions;

using FastEndpoints;

namespace WebApi.Files.GetColumnTypeCatalog;

public class GetColumnTypeCatalogEndpoint : EndpointWithoutRequest<GetColumnTypeCatalogResponse>
{
    private readonly ITypeCatalogService _typeCatalog;

    public GetColumnTypeCatalogEndpoint(ITypeCatalogService typeCatalog)
    {
        _typeCatalog = typeCatalog;
    }

    public override void Configure()
    {
        Get("api/types/columns");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get supported column data types and aliases";
            s.Description = "Returns list of allowed canonical types and their aliases for column type mapping.";
            s.ResponseExamples[200] = new GetColumnTypeCatalogResponse
            {
                AllowedTypes = new[] { "string", "int", "long", "decimal", "bool", "datetime" },
                TypeAliases = new Dictionary<string, string>
                {
                    ["integer"] = "int",
                    ["number"] = "decimal",
                    ["boolean"] = "bool",
                    ["date"] = "datetime"
                }
            };
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = new GetColumnTypeCatalogResponse
        {
            AllowedTypes = _typeCatalog.GetAllowedTypes(),
            TypeAliases = _typeCatalog.GetTypeAliases()
        };

        // await SendAsync(ct);
    }
}