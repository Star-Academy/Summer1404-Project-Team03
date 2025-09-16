namespace WebApi.Workflow.ListWorkFlows;

public class ListWorkflowsResponse
{
    public List<WorkflowItem> Workflows { get; set; } = new();
}

public class WorkflowItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TableId { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}