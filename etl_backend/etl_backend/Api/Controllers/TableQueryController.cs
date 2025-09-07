using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tables")]
public class TablesQueryController : ControllerBase
{
    private readonly ITableManagementService _mgmt;
    private readonly ITableInfoService _info;

    public TablesQueryController(ITableManagementService mgmt, ITableInfoService info)
    {
        _mgmt = mgmt;
        _info = info;
    }

    public sealed class TableListItem
    {
        public int SchemaId { get; set; }
        public string TableName { get; set; } = default!;
        public string OriginalFileName { get; set; } = "";
        public int ColumnCount { get; set; }
    }
    

    [HttpGet("{schemaId:int}/details")]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int schemaId, CancellationToken ct = default)
    {
        try
        {
            var dto = await _info.GetDetailsAsync(schemaId, ct);
            return Ok(dto);
        }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        catch (Exception ex) { return Problem(statusCode: 500, title: "Get details failed.", detail: ex.Message); }
    }

    [HttpGet("{schemaId:int}/rows")]
    [AllowAnonymous]
    public async Task<IActionResult> Rows(
        int schemaId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 50,
        [FromQuery] string? orderBy = null,
        [FromQuery] string? direction = null,
        CancellationToken ct = default)
    {
        try
        {
            var dto = await _info.PreviewRowsAsync(schemaId, offset, limit, orderBy, direction, ct);
            return Ok(dto);
        }
        catch (ArgumentException ex) { return UnprocessableEntity(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
        catch (Exception ex) { return Problem(statusCode: 500, title: "Preview rows failed.", detail: ex.Message); }
    }

    [HttpGet("{schemaId:int}/count")]
    [AllowAnonymous]
    public async Task<IActionResult> Count(int schemaId, [FromQuery] bool exact = false, CancellationToken ct = default)
    {
        try
        {
            var dto = await _info.GetRowCountAsync(schemaId, exact, ct);
            return Ok(dto);
        }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
        catch (Exception ex) { return Problem(statusCode: 500, title: "Get count failed.", detail: ex.Message); }
    }
}
