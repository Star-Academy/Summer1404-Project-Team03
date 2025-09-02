using etl_backend.Api.Dtos;
using etl_backend.Application.DataFile.Services;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Api.Controllers;

using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileStagingService _staging;
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IHeaderProvider _headers;
    private readonly IColumnDefinitionBuilder _cols;
    private readonly ISchemaRegistrationService _schemaReg;

    public FilesController(
        IFileStagingService staging,
        IStagedFileRepository stagedRepo,
        IHeaderProvider headers,
        IColumnDefinitionBuilder cols,
        ISchemaRegistrationService schemaReg)
    {
        _staging = staging;
        _stagedRepo = stagedRepo;
        _headers = headers;
        _cols = cols;
        _schemaReg = schemaReg;
    }

    [HttpPost]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 512_000_000)]
    public async Task<ActionResult<StageFileResponse>> Stage(
        [FromForm] StageFileRequest request,
        [FromQuery] string? subdir = "uploads",
        CancellationToken ct = default)
    {
        if (request.File is null || request.File.Length == 0)
            return BadRequest("file is missing or empty.");

        await using var stream = request.File.OpenReadStream();
        var staged = await _staging.StageAsync(stream, request.File.FileName, subdir, ct);

        var dto = new StageFileResponse
        {
            Id = staged.Id,
            OriginalFileName = staged.OriginalFileName,
            StoredFilePath = staged.StoredFilePath,
            FileSize = staged.FileSize,
            UploadedAt = staged.UploadedAt,
            Stage = staged.Stage.ToString(),
            Status = staged.Status.ToString(),
            ErrorCode = staged.ErrorCode.ToString(),
            ErrorMessage = staged.ErrorMessage
        };
        return Ok(dto);
    }

    [HttpGet("{id:int}/schema/preview")]
    [AllowAnonymous]
    public async Task<ActionResult<ColumnPreviewResponse>> PreviewSchema([FromRoute] int id, CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(id, ct);
        if (staged is null) return NotFound();

        if (staged.Status == ProcessingStatus.Failed)
            return Conflict("staged file is in failed state.");

        var headerNames = await _headers.GetAsync(staged, ct);
        if (headerNames.Count == 0)
            return UnprocessableEntity("header row not found or empty.");

        var columnEntities = _cols.Build(headerNames);

        var response = new ColumnPreviewResponse
        {
            StagedFileId = staged.Id,
            Columns = columnEntities
                .OrderBy(c => c.OrdinalPosition)
                .Select(c => new ColumnPreviewItem
                {
                    OrdinalPosition = c.OrdinalPosition,
                    ColumnName = c.ColumnName,
                    OriginalColumnName = c.OriginalColumnName
                })
                .ToList()
        };
        return Ok(response);
    }


    [HttpPost("{id:int}/schema/register")]
    [AllowAnonymous]
    [Consumes("application/json")]
    public async Task<ActionResult> RegisterSchema([FromRoute] int id, [FromBody] RegisterSchemaRequest request, CancellationToken ct = default)
    {
        if (request?.Columns is null || request.Columns.Count == 0)
            return BadRequest("columns are required.");

        var staged = await _stagedRepo.GetByIdAsync(id, ct);
        if (staged is null) return NotFound();

        if (staged.Stage == ProcessingStage.TableCreated || staged.Stage == ProcessingStage.Loaded)
            return Conflict("columns cannot be modified after the table has been created.");

        var map = request.Columns
            .GroupBy(x => x.OrdinalPosition)
            .ToDictionary(g => g.Key, g => g.Last().ColumnType);

        var (schema, updatedStaged) = await _schemaReg.RegisterAsync(id, map, ct);

        return Ok(new
        {
            SchemaId = schema.Id,
            TableName = schema.TableName,
            Columns = schema.Columns
                .OrderBy(c => c.OrdinalPosition)
                .Select(c => new { c.OrdinalPosition, c.ColumnName, c.ColumnType })
                .ToList(),
            Staged = new
            {
                updatedStaged.Id,
                Stage = updatedStaged.Stage.ToString(),
                Status = updatedStaged.Status.ToString(),
                ErrorCode = updatedStaged.ErrorCode.ToString()
            }
        });
    }

    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ListFilesItem>>> ListFiles(CancellationToken ct = default)
    {
        var items = await _stagedRepo.ListAsync(ct);
        var result = items.Select(s => new ListFilesItem
        {
            Id = s.Id,
            OriginalFileName = s.OriginalFileName,
            Stage = s.Stage.ToString(),
            Status = s.Status.ToString(),
            SchemaId = s.SchemaId
        }).ToList();

        return Ok(result);
    }
}
