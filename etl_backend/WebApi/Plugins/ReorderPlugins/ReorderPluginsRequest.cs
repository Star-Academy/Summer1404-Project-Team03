namespace WebApi.Plugins.ReorderPlugins;

public class ReorderPluginsRequest
{
    public List<string> PluginIdsInOrder { get; set; } = new();
}