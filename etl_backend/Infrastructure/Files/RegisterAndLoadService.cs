using Application.Abstractions;
using Application.Files.Commands;
using Application.Files.Services;
using Application.Repositories.Abstractions;
using MediatR;

namespace Infrastructure.Files;

public class RegisterAndLoadService : IRegisterAndLoadService
{
    private readonly IMediator _mediator;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IFileStagingService _fileStagingService;

    public RegisterAndLoadService(
        IMediator mediator,
        IDataTableSchemaRepository schemaRepo,
        IStagedFileRepository stagedRepo,
        IFileStagingService fileStagingService)
    {
        _mediator = mediator;
        _schemaRepo = schemaRepo;
        _stagedRepo = stagedRepo;
        _fileStagingService = fileStagingService;
    }

    public async Task<RegisterAndLoadResult> ExecuteAsync(
        int stagedFileId,
        Dictionary<int, string> columnTypeMap,
        Dictionary<int, string> columnNameMap,
        LoadMode mode = LoadMode.Append,
        bool dropOnFailure = false,
        CancellationToken ct = default)
    {
        RegisterSchemaResult? registerResult = null;

        try
        {
            registerResult = await _mediator.Send(new RegisterSchemaCommand(stagedFileId, columnTypeMap, columnNameMap), ct);
            
            var loadResult = await _mediator.Send(new LoadFileIntoTableCommand(stagedFileId, mode, dropOnFailure), ct);

            return new RegisterAndLoadResult(
                registerResult.SchemaId,
                registerResult.TableName,
                registerResult.Columns,
                registerResult.Staged,
                new LoadResult(loadResult.RowsInserted, loadResult.ElapsedMs)
            );
        }
        catch (Exception ex)
        {
            if (registerResult != null)
            {
                await CompensateAsync(registerResult.SchemaId, stagedFileId, ct);
            }

            throw new ApplicationException("Register and load operation failed. Compensated.", ex);
        }
    }

    private async Task CompensateAsync(int schemaId, int stagedFileId, CancellationToken ct)
    {
        try
        {
            var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct);
            var filePath = staged?.StoredFilePath;
            
            await _schemaRepo.DeleteAsync(schemaId, ct);
            
            await _stagedRepo.DeleteAsync(stagedFileId, ct);
            if (!string.IsNullOrEmpty(filePath))
            {
                await _fileStagingService.DeleteAsync(filePath, ct);
            }
        }
        catch (Exception compensationEx)
        {
            Console.WriteLine($"Compensation failed: {compensationEx.Message}");
        }
    }
}