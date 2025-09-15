using Domain.Entities;

namespace Application.WorkFlow.Abstractions;

public interface IWorkflowReader
{
    Task<Workflow?> GetByIdAsync(string id, string userId, CancellationToken ct);
    Task<List<Workflow>> ListByUserAsync(string userId, CancellationToken ct);
}