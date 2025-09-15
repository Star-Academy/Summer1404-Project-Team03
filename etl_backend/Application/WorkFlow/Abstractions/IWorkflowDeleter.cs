namespace Application.WorkFlow.Abstractions;

public interface IWorkflowDeleter
{
    Task DeleteAsync(string id, string userId, CancellationToken ct);
}