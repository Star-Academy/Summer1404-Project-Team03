using Domain.Enums;

namespace Domain.Entities;

public class Workflow
{
    public string Id { get; private set; }
    public string UserId { get; private set; } 
    public string? TableId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public WorkflowStatus Status { get; private set; }

    private readonly List<Plugin> _plugins = new();
    public IReadOnlyList<Plugin> Plugins => _plugins.AsReadOnly();

    private Workflow() { }

    public Workflow(
        string id,
        string userId, 
        string ? tableId,
        string name,
        string? description = null,
        WorkflowStatus status = WorkflowStatus.Draft)
    {
        Id = id;
        UserId = userId;
        TableId = tableId;
        Name = name;
        Description = description;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, WorkflowStatus status, string tableId)
    {
        Name = name;
        TableId = tableId;
        Description = description;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddPlugin(Plugin plugin, int? order = null)
    {
        plugin.WorkflowId = Id;
    
        if (order.HasValue)
        {
            var index = Math.Max(0, Math.Min(order.Value - 1, _plugins.Count));
            _plugins.Insert(index, plugin);
        
            for (int i = index; i < _plugins.Count; i++)
            {
                _plugins[i].UpdateOrder(i + 1);
            }
        }
        else
        {
            plugin.UpdateOrder(_plugins.Count + 1);
            _plugins.Add(plugin);
        }
    
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemovePlugin(string pluginId)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Id == pluginId);
        if (plugin != null)
        {
            _plugins.Remove(plugin);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void ReorderPlugins(List<string> pluginIdsInOrder)
    {
        var newOrder = new List<Plugin>();
        foreach (var id in pluginIdsInOrder)
        {
            var plugin = _plugins.FirstOrDefault(p => p.Id == id);
            if (plugin != null)
            {
                newOrder.Add(plugin);
            }
        }
        _plugins.Clear();
        _plugins.AddRange(newOrder);
        for (int i = 0; i < _plugins.Count; i++)
        {
            _plugins[i].UpdateOrder(i + 1);
        }
        UpdatedAt = DateTime.UtcNow;
    }
}