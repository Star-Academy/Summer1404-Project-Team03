using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Application.Plugins.Dtos;

public record PluginDto(
    string Id,
    PluginType PluginType,
    PluginConfig Config, 
    int Order,
    DateTime CreatedAt,
    DateTime UpdatedAt
);