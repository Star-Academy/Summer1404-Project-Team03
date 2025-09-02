using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace etl_backend.Api.Controllers;

[ApiController]
[Route("api/tables/{schemaId:int}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly IColumnManagementService _svc;

    public ColumnsController(IColumnManagementService svc) => _svc = svc;

    public sealed class RenameColumnRequest
    {
        public string NewName { get; set; } = default!;
    }

    public sealed class DropColumnsRequest
    {
        public List<int> ColumnIds { get; set; } = new();
    }

    public sealed class ColumnListItem
    {
        public int Id { get; set; }
        public int OrdinalPosition { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? OriginalName { get; set; }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ColumnListItem>>> List(int schemaId, CancellationToken ct = default)
    {
        try
        {
            var cols = await _svc.ListAsync(schemaId, ct);
            var dto = cols.Select(c => new ColumnListItem
            {
                Id = c.Id,
                OrdinalPosition = c.OrdinalPosition,
                Name = c.ColumnName,
                Type = c.ColumnType,
                OriginalName = c.OriginalColumnName
            })
            .OrderBy(c => c.OrdinalPosition)
            .ToList();

            return Ok(dto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return Problem(statusCode: 500, title: "List columns failed.", detail: ex.Message);
        }
    }

    [HttpPost("{columnId:int}/rename")]
    [AllowAnonymous]
    public async Task<IActionResult> Rename(int schemaId, int columnId, [FromBody] RenameColumnRequest req, CancellationToken ct = default)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.NewName))
            return BadRequest(new { error = "NewName is required." });

        try
        {
            await _svc.RenameAsync(schemaId, columnId, req.NewName, ct);
            return NoContent();
        }
        catch (ArgumentException ex)       { return UnprocessableEntity(new { error = ex.Message }); }
        catch (InvalidOperationException ex){ return Conflict(new { error = ex.Message }); }
        catch (Exception ex)               { return Problem(statusCode: 500, title: "Rename column failed.", detail: ex.Message); }
    }

    [HttpDelete]
    [AllowAnonymous]
    public async Task<IActionResult> Drop(int schemaId, [FromBody] DropColumnsRequest req, CancellationToken ct = default)
    {
        if (req?.ColumnIds is null || req.ColumnIds.Count == 0)
            return BadRequest(new { error = "ColumnIds are required." });

        try
        {
            await _svc.DropAsync(schemaId, req.ColumnIds, ct);
            return NoContent();
        }
        catch (ArgumentException ex)       { return UnprocessableEntity(new { error = ex.Message }); }
        catch (InvalidOperationException ex){ return Conflict(new { error = ex.Message }); }
        catch (Exception ex)               { return Problem(statusCode: 500, title: "Drop columns failed.", detail: ex.Message); }
    }
}
