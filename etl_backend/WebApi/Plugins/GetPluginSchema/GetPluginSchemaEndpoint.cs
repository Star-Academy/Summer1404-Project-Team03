using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Domain.Enums;
using FastEndpoints;

namespace WebApi.Plugins.GetPluginSchema;

public class GetPluginSchemaEndpoint : EndpointWithoutRequest<object>
{
    private readonly IEnumerable<IPluginSchemaProvider> _schemaProviders;

    public GetPluginSchemaEndpoint(IEnumerable<IPluginSchemaProvider> schemaProviders)
    {
        _schemaProviders = schemaProviders;
    }

    public override void Configure()
    {
        Get("api/plugins/{pluginType}/schema");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get schema for a plugin type";
            s.Description = "Returns JSON schema for the specified plugin type.";
            // s.Params("pluginType", "Plugin type (Filter, Aggregate)", example: "Filter");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var pluginTypeStr = Route<string>("pluginType");
        if (!Enum.TryParse<PluginType>(pluginTypeStr, true, out var pluginType))
        {
            AddError($"Invalid plugin type: {pluginTypeStr}");
            await SendErrorsAsync(400, ct);
            return;
        }

        var provider = _schemaProviders.FirstOrDefault(p => p.PluginType == pluginType)
                       ?? throw new NotFoundException("Plugin schema provider", pluginTypeStr);

        Response = provider.GetSchema();
    }
}