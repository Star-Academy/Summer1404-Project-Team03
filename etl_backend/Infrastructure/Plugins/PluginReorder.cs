using Application.Plugins.Abstractions;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Plugins;

public class PluginReorderer : IPluginReorderer
{
    private readonly IEtlDbContextFactory _contextFactory;

    public PluginReorderer(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task ReorderAsync(string workflowId, List<string> pluginIdsInOrder, string userId, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();

        var workflow = await ctx.Workflows
            .FirstOrDefaultAsync(w => w.Id == workflowId && w.UserId == userId, ct);

        if (workflow == null)
            return; 

        var plugins = await ctx.Plugins
            .Where(p => p.WorkflowId == workflowId)
            .ToListAsync(ct);

        for (int i = 0; i < pluginIdsInOrder.Count; i++)
        {
            var plugin = plugins.FirstOrDefault(p => p.Id == pluginIdsInOrder[i]);
            if (plugin != null)
            {
                plugin.UpdateOrder(i + 1); 
            }
        }

        await ctx.SaveChangesAsync(ct);
    }
}