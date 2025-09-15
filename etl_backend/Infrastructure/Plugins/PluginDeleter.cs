using Application.Plugins.Abstractions;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Plugins;

public class PluginDeleter : IPluginDeleter
{
    private readonly IEtlDbContextFactory _contextFactory;

    public PluginDeleter(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task DeleteAsync(string id, string userId, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();

        var plugin = await ctx.Plugins
            .Join(ctx.Workflows,
                p => p.WorkflowId,
                w => w.Id,
                (p, w) => new { Plugin = p, Workflow = w })
            .Where(x => x.Plugin.Id == id && x.Workflow.UserId == userId)
            .Select(x => x.Plugin)
            .FirstOrDefaultAsync(ct);

        if (plugin != null)
        {
            ctx.Plugins.Remove(plugin);
            await ctx.SaveChangesAsync(ct);
        }
    }
}