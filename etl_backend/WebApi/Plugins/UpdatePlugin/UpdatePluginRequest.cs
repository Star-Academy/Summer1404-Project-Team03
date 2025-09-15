using Domain.Enums;

namespace WebApi.Plugins.UpdatePlugin;

public class UpdatePluginRequest
{
    public PluginType PluginType { get; set; }
    public Dictionary<string, object> Config { get; set; } = new();
}