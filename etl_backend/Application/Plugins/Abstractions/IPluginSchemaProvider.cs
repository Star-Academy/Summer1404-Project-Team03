using Domain.Enums;

namespace Application.Plugins.Abstractions;

public interface IPluginSchemaProvider
{
    PluginType PluginType { get; }
    object GetSchema();
}