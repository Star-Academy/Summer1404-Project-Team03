using FastEndpoints;

namespace WebApi.Tables.RenameTable;

public class RenameTableRequest
{
    [BindFrom("SchemaId")] public int SchemaId { get; set; }
    public string? NewTableName { get; set; }
}