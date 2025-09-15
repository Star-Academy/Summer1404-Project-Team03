using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Application.Plugins;

public class PluginConfigValidationService
{
    private readonly IEnumerable<IPluginConfigValidator> _validators;

    public PluginConfigValidationService(IEnumerable<IPluginConfigValidator> validators)
    {
        _validators = validators;
    }

    public PluginConfig Validate(PluginType pluginType, Dictionary<string, object> rawConfig)
    {
        var validator = _validators.FirstOrDefault(v => v.CanValidate(pluginType))
                        ?? throw new UnprocessableEntityException($"No validator found for plugin type: {pluginType}");

        return validator.ValidateAndParse(rawConfig);
    }
}