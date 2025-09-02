using etl_backend.Application.DataFile.Services;

namespace etl_backend.Api.Controllers;

using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/files")]
public class LoadController : ControllerBase
{
    private readonly ITableLoadService _tableLoad;
    private readonly ILoadPolicyFactory _loadPolicyFactory;

    public LoadController(ITableLoadService tableLoad, ILoadPolicyFactory loadPolicyFactory)
    {
        _tableLoad = tableLoad;
        _loadPolicyFactory = loadPolicyFactory;
    }

    [HttpPost("{id:int}/load")]
    [AllowAnonymous]
    public async Task<IActionResult> Load(
        [FromRoute] int id,
        [FromQuery] LoadMode mode = LoadMode.Append,
        [FromQuery] bool dropOnFailure = false,
        CancellationToken ct = default)
    {
        var policy = _loadPolicyFactory.Create(mode, dropOnFailure);

        try
        {
            var result = await _tableLoad.LoadAsync(id, policy, ct);
            return Ok(new
            {
                Rows = result.RowsInserted,
                ElapsedMs = result.Duration.TotalMilliseconds
            });
        }
        catch (OperationCanceledException)
        {
            return Problem(statusCode: StatusCodes.Status499ClientClosedRequest, title: "Request was cancelled.");
        }
        catch (ArgumentException ex)
        {
            return UnprocessableEntity(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Load failed.", detail: ex.Message);
        }
    }
}
