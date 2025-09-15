using Domain.Enums;
using Domain.ValueObjects.PluginConfig;

namespace Domain.Entities;

public class Plugin
{
    public string Id { get; private set; }
    public string WorkflowId { get; internal set; } 
    public PluginType PluginType { get; private set; }  
    public PluginConfig Config { get; private set; }
    public int Order { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Plugin() { }

    public Plugin(
        string id,
        PluginType pluginType,
        PluginConfig config,
        int order)
    {
        Id = id;
        PluginType = pluginType;
        Config = config;
        Order = order;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(PluginType pluginType, PluginConfig config)
    {
        PluginType = pluginType;
        Config = config;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOrder(int order)
    {
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }
}