using Application.Common.Authorization;
using MediatR;

namespace Application.Files.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record LoadFileIntoTableCommand(
    int StagedFileId,
    LoadMode Mode = LoadMode.Append,
    bool DropOnFailure = false
) : IRequest<LoadResult>;

public record LoadResult(
    long RowsInserted,
    double ElapsedMs
);
public enum LoadMode
{
    FailIfExists,
    Replace,
    Append
}