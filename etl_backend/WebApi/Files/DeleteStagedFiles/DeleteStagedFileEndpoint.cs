

using Application.Files.Commands;
using FastEndpoints;
using MediatR;

namespace WebApi.Files;

/// <summary>
/// Delete a staged file and its physical storage
/// </summary>
/// <remarks>
/// Deletes both database record and physical file. Cannot delete if already loaded.
/// </remarks>
public class DeleteStagedFileEndpoint : Endpoint<DeleteStagedFileRequest>
{
    private readonly IMediator _mediator;

    public DeleteStagedFileEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        // Route that expects 'id' in the URL
        Delete("api/files/{id}");

        AllowAnonymous();
        
        Summary(s =>
        {
            s.Summary = "Delete staged file";
            s.Description = "Deletes staged file record and physical file. Cannot delete if already loaded.";
            // s.Params("id", "Staged file ID", example: 1);
            // s.ExampleRequest = new DeleteStagedFileRequest { Id = 1 };
        });
        // Summary(s => s.Params("id", "Staged file ID", example: 1));
        
    }
    public override async Task HandleAsync(DeleteStagedFileRequest req, CancellationToken ct)
    {
        // Pass the request to the mediator to handle the command
        await _mediator.Send(new DeleteStagedFileCommand(req.Id), ct);
        
    }
}