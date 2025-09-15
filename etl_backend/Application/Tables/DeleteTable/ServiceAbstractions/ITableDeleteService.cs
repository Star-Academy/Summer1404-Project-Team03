namespace Application.Tables.DeleteTable.ServiceAbstractions;

public interface ITableDeleteService
{
    Task DeleteAsync(int schemaId, CancellationToken ct = default);
}