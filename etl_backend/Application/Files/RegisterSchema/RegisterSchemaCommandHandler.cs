using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Commands;
using Application.Services.Repositories.Abstractions;
using Domain.Enums;
using MediatR;

namespace Application.Files.Handlers;

public class RegisterSchemaCommandHandler : IRequestHandler<RegisterSchemaCommand, RegisterSchemaResult>
{
    private readonly ISchemaRegistrationService _schemaReg;

    public RegisterSchemaCommandHandler(
        ISchemaRegistrationService schemaReg)
    {
        _schemaReg = schemaReg;
    }

    public async Task<RegisterSchemaResult> Handle(RegisterSchemaCommand request, CancellationToken ct)
    {

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