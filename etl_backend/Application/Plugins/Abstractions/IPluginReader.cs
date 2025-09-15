using Domain.Entities;

namespace Application.Plugins.Abstractions;

public interface IPluginReader
{
    Task<Plugin?> GetByIdAsync(string id, string userId, CancellationToken ct);
    Task<List<Plugin>> GetByWorkflowIdAsync(string workflowId, string userId, CancellationToken ct);
}