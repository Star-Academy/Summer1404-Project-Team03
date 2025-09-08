using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Commands;
using MediatR;

namespace Application.Files.Handlers;

public class LoadFileIntoTableCommandHandler : IRequestHandler<LoadFileIntoTableCommand, LoadResult>
{
    private readonly ITableLoadService _tableLoad;
    private readonly ILoadPolicyFactory _loadPolicyFactory;

    public LoadFileIntoTableCommandHandler(
        ITableLoadService tableLoad,
        ILoadPolicyFactory loadPolicyFactory)
    {
        _tableLoad = tableLoad;
        _loadPolicyFactory = loadPolicyFactory;
    }

    public async Task<LoadResult> Handle(LoadFileIntoTableCommand request, CancellationToken ct)
    {
        var policy = _loadPolicyFactory.Create(request.Mode, request.DropOnFailure);

        try
        {
            var result = await _tableLoad.LoadAsync(request.StagedFileId, policy, ct);

            return new LoadResult(
                RowsInserted: result.RowsInserted,
                ElapsedMs: result.ElapsedMs
            );
        }
        catch (ArgumentException ex)
        {
            throw new UnprocessableEntityException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new ConflictException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Load failed.", ex);
        }
    }
}