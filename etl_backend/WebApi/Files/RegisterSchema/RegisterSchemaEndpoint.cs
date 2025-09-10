using Application.Files.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Files;

/// <summary>
/// Register column schema for a staged file
/// </summary>
/// <remarks>
/// Assigns column types and registers the schema in the system. Prepares for table creation.
/// </remarks>
public class RegisterSchemaEndpoint : Endpoint<RegisterSchemaRequest, RegisterSchemaResponse>
{
    private readonly IMediator _mediator;

    public RegisterSchemaEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("api/files/{Id}/schema/register");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Register schema for staged file";
            s.Description = "Assigns column types and registers schema. Cannot be modified after table creation.";
            s.ExampleRequest = new RegisterSchemaRequest
            {
                Id = 1,
                Columns = new List<RegisterSchemaColumnItem>
                {
                    new() { OrdinalPosition = 1, ColumnType = "string", ColumnName = "new_name"}
                }
            };
        });
    }

    public override async Task HandleAsync(RegisterSchemaRequest req, CancellationToken ct)
    {
        var columnMap = req.Columns
            .ToDictionary(c => c.OrdinalPosition, c => c.ColumnType);
        var columnNameMap = req.Columns
            .ToDictionary(c => c.OrdinalPosition, c => c.ColumnName);
        var result = await _mediator.Send(new RegisterSchemaCommand(req.Id, columnMap, columnNameMap), ct);

        Response = new RegisterSchemaResponse
        {
            SchemaId = result.SchemaId,
            TableName = result.TableName,
            Columns = result.Columns.Select(c => new RegisterSchemaColumnResponse
            {
                OrdinalPosition = c.OrdinalPosition,
                ColumnName = c.ColumnName,
                ColumnType = c.ColumnType.ToString()
            }).ToList(),
            Staged = new StagedFileStatusResponse
            {
                Id = result.Staged.Id,
                Stage = result.Staged.Stage,
                Status = result.Staged.Status,
                ErrorCode = result.Staged.ErrorCode
            }
        };
    }
}