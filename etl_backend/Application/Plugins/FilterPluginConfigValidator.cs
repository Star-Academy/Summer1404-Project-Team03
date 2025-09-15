using System.Text.Json;
using Application.Common.Exceptions;
using Application.Common.Mappers;
using Application.Plugins.Abstractions;
using Application.Plugins.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Application.Plugins;

public class FilterPluginConfigValidator : IPluginConfigValidator
{
    public bool CanValidate(PluginType pluginType) => pluginType == PluginType.Filter;

    public PluginConfig ValidateAndParse(Dictionary<string, object> rawConfig)
    {
        if (!rawConfig.TryGetValue("Conditions", out var conditionsObj))
        {
            throw new UnprocessableEntityException("Invalid FilterConfig: Missing Conditions.");
        }

        var conditionsJson = JsonSerializer.Serialize(conditionsObj);
        var conditionsDto = JsonSerializer.Deserialize<List<FilterConditionDto>>(conditionsJson)
                            ?? throw new UnprocessableEntityException("Invalid FilterConfig: Failed to parse Conditions.");

        var conditions = conditionsDto.Select(dto => dto.ToDomain()).ToArray();

        return new FilterConfig(conditions);
    }
}