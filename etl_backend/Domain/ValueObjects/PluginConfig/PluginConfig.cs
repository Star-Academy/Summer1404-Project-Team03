using System.Text.Json.Serialization;

namespace Domain.ValueObjects.PluginConfig;

public class PluginConfig 
{
    [JsonConstructor]
    public PluginConfig()
    {
    }
}