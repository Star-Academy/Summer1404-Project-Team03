using Application.Plugins.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Plugins;

public class PluginReader : IPluginReader
{
    private readonly IEtlDbContextFactory _contextFactory;

    public PluginReader(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Plugin?> GetByIdAsync(string id, string userId, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();
        
        return await ctx.Plugins
            .Join(ctx.Workflows,
                plugin => plugin.WorkflowId,
                workflow => workflow.Id,
                (plugin, workflow) => new { Plugin = plugin, Workflow = workflow })
            .Where(x => x.Plugin.Id == id && x.Workflow.UserId == userId)
            .Select(x => x.Plugin)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Plugin>> GetByWorkflowIdAsync(string workflowId, string userId, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();
        
        return await ctx.Plugins
            .Join(ctx.Workflows,
                plugin => plugin.WorkflowId,
                workflow => workflow.Id,
                (plugin, workflow) => new { Plugin = plugin, Workflow = workflow })
            .Where(x => x.Plugin.WorkflowId == workflowId && x.Workflow.UserId == userId)
            .OrderBy(x => x.Plugin.Order)
            .Select(x => x.Plugin)
            .ToListAsync(ct);
    }
}