using Application.Common.Authorization;
using Application.Dtos;
using Application.Enums;
using MediatR;

namespace Application.Files.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record LoadFileIntoTableCommand(
    int StagedFileId,
    LoadMode Mode = LoadMode.Append,
    bool DropOnFailure = false
) : IRequest<LoadResult>;


