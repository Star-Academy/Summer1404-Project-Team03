using Microsoft.AspNetCore.Mvc;

namespace WebApi.Tables.DropColumns;

public class DropColumnsRequest
{
    [FromRoute]
    public int SchemaId { get; set; }
    [FromBody]
    public List<int> ColumnIds { get; set; } = new();
}