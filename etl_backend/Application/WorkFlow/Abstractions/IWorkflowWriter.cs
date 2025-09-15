using Domain.Entities;

namespace Application.WorkFlow.Abstractions;

public interface IWorkflowWriter
{
    Task AddAsync(Workflow workflow, CancellationToken ct);
    Task UpdateAsync(Workflow workflow, CancellationToken ct);
}