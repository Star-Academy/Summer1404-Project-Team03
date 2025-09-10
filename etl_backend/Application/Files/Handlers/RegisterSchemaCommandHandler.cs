using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Commands;
using Application.Repositories.Abstractions;
using Domain.Enums;
using MediatR;

namespace Application.Files.Handlers;

public class RegisterSchemaCommandHandler : IRequestHandler<RegisterSchemaCommand, RegisterSchemaResult>
{
    private readonly ISchemaRegistrationService _schemaReg;
    private readonly IStagedFileRepository _stagedRepo;

    public RegisterSchemaCommandHandler(
        ISchemaRegistrationService schemaReg,
        IStagedFileRepository stagedRepo)
    {
        _schemaReg = schemaReg;
        _stagedRepo = stagedRepo;
    }

    public async Task<RegisterSchemaResult> Handle(RegisterSchemaCommand request, CancellationToken ct)
    {
        var staged = await _stagedRepo.GetByIdAsync(request.StagedFileId, ct);
        if (staged is null)
            throw new NotFoundException("StagedFile", request.StagedFileId);

        if (staged.Stage == ProcessingStage.TableCreated || staged.Stage == ProcessingStage.Loaded)
            throw new ConflictException("Columns cannot be modified after the table has been created.");

        var (schema, updatedStaged) = await _schemaReg.RegisterAsync(request.StagedFileId, request.ColumnTypeMap,request.ColumnNameMap, ct);

        var columns = schema.Columns
            .OrderBy(c => c.OrdinalPosition)
            .Select(c => new SchemaColumnDto(
                c.OrdinalPosition,
                c.ColumnName,
                c.ColumnType
            ))
            .ToList();

        var stagedStatus = new StagedFileStatusDto(
            updatedStaged.Id,
            updatedStaged.Stage.ToString(),
            updatedStaged.Status.ToString(),
            updatedStaged.ErrorCode == ProcessingErrorCode.None ? null : updatedStaged.ErrorCode.ToString()
        );

        return new RegisterSchemaResult(
            schema.Id,
            schema.TableName,
            columns,
            stagedStatus
        );
    }
}