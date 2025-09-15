namespace Application.WorkFlow.Dtos;

public record WorkflowDto(
    string Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Status // "Draft", "Running", "Completed", "Failed"
);