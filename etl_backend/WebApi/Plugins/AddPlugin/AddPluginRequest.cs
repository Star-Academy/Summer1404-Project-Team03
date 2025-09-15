using Domain.Enums;

namespace WebApi.Plugins.AddPlugin;

public class AddPluginRequest
{
    public PluginType PluginType { get; set; }
    public Dictionary<string, object> Config { get; set; } = new();
    public int? Order { get; set; }
}