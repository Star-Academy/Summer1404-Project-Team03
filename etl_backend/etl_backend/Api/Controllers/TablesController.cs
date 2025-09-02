using etl_backend.Api.Dtos;
using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Api.Controllers;

using etl_backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tables")]
public class TablesController : ControllerBase
{
    private readonly ITableManagementService _svc;

    public TablesController(ITableManagementService svc) => _svc = svc;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TableListItem>>> List(
        [FromQuery] bool onlyPhysical = false,
        CancellationToken ct = default)
    {
        var data = await _svc.ListAsync(onlyPhysical, ct);
        var result = data
            .OrderByDescending(s => s.Id)
            .Select(s => new TableListItem
            {
                SchemaId = s.Id,
                TableName = s.TableName,
                OriginalFileName = s.OriginalFileName ?? "",
                ColumnCount = s.Columns?.Count ?? 0
            })
            .ToList();
        return Ok(result);
    }

    [HttpPost("{schemaId:int}/rename")]
    [AllowAnonymous]
    public async Task<IActionResult> Rename(int schemaId, [FromBody] RenameTableRequest req, CancellationToken ct = default)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.NewTableName))
            return BadRequest(new { error = "NewTableName is required." });

        try
        {
            await _svc.RenameAsync(schemaId, req.NewTableName, ct);
            return NoContent();
        }
        catch (ArgumentException ex) { return UnprocessableEntity(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        catch (Exception ex) { return Problem(statusCode: 500, title: "Rename failed.", detail: ex.Message); }
    }

    [HttpDelete("{schemaId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(int schemaId, CancellationToken ct = default)
    {
        try
        {
            await _svc.DeleteAsync(schemaId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        catch (Exception ex) { return Problem(statusCode: 500, title: "Delete failed.", detail: ex.Message); }
    }
}


