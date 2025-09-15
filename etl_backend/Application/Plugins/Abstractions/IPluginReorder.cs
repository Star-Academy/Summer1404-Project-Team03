namespace Application.Plugins.Abstractions;

public interface IPluginReorderer
{
    Task ReorderAsync(string workflowId, List<string> pluginIdsInOrder, string userId, CancellationToken ct);
}