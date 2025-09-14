using Application.Abstractions;
using Application.Dtos;
using Application.Enums;
using Application.Files.Commands;
using Application.Files.DeleteStagedFile.ServiceAbstractions;
using Application.Files.StageManyFiles.ServiceAbstractions;
using Application.Services.Repositories.Abstractions;
using MediatR;

namespace Infrastructure.Files;

public class RegisterAndLoadService : IRegisterAndLoadService
{
    private readonly IMediator _mediator;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDeleteStagedFile _deleteFileStagingService;

    public RegisterAndLoadService(
        IMediator mediator,
        IDataTableSchemaRepository schemaRepo,
        IStagedFileRepository stagedRepo,
        IDeleteStagedFile fileStagingService)
    {
        _mediator = mediator;
        _schemaRepo = schemaRepo;
        _stagedRepo = stagedRepo;
        _deleteFileStagingService = fileStagingService;
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
                await _deleteFileStagingService.DeleteAsync(filePath, ct);
            }
        }
        catch (Exception compensationEx)
        {
            Console.WriteLine($"Compensation failed: {compensationEx.Message}");
        }
    }
}