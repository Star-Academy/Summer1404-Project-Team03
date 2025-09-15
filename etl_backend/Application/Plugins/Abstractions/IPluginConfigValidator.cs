using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Application.Plugins.Abstractions;

public interface IPluginConfigValidator
{
    bool CanValidate(PluginType pluginType);
    PluginConfig ValidateAndParse(Dictionary<string, object> rawConfig);
}