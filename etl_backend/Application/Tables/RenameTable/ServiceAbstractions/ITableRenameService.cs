namespace Application.Tables.RenameTable.ServiceAbstractions;

public interface ITableRenameService
{
    Task RenameAsync(int schemaId, string newTableName, CancellationToken ct = default);
}