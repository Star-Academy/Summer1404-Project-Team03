using Domain.Entities;

namespace Application.Plugins.Abstractions;

public interface IPluginWriter
{
    Task AddAsync(Plugin plugin, CancellationToken ct);
    Task UpdateAsync(Plugin plugin, CancellationToken ct);
}