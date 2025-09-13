using Application.Abstractions;
using Application.Files.Commands;
using FastEndpoints;

namespace WebApi.Files.RegisterAndLoad;

public class RegisterAndLoadEndpoint : Endpoint<RegisterAndLoadRequest, RegisterAndLoadResponse>
{
    private readonly IRegisterAndLoadService _service;

    public RegisterAndLoadEndpoint(IRegisterAndLoadService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("api/files/{Id}/register-and-load");
        Summary(s =>
        {
            s.Summary = "Register Schema and load the data into database table";
            s.Description = "Returns approximate or exact row count.";
            // s.Params(p => p.SchemaId, "The schema ID of the table");
            s.ExampleRequest = new RegisterAndLoadRequest { LoadMode = "Append", DropOnFailure = true};
        });
    }

    public override async Task HandleAsync(RegisterAndLoadRequest req, CancellationToken ct)
    {
        var columnMap = req.Columns
            .ToDictionary(c => c.OrdinalPosition, c =>  c.ColumnType);
        var columnNameMap = req.Columns
            .ToDictionary(c => c.OrdinalPosition, c =>  c.ColumnType);
        var result = await _service.ExecuteAsync(
            req.Id,
            columnMap,
            columnNameMap,
            (LoadMode)Enum.Parse(typeof(LoadMode), req.LoadMode, true),
            req.DropOnFailure,
            ct
        );

        Response = new RegisterAndLoadResponse
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
            },
            Load = new LoadFileIntoTableResponse
            {
                RowsInserted = result.Load.RowsInserted,
                ElapsedMs = result.Load.ElapsedMs
            }
        };
    }
}