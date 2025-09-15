using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Application.Plugins.Abstractions;

public interface IPluginConfigParser
{
    PluginConfig Parse(PluginType pluginType, object config);
}