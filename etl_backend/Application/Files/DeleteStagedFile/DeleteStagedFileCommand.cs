using Application.Common.Authorization;
using MediatR;

namespace Application.Files.Commands;
[RequireRole(AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record DeleteStagedFileCommand(Guid StagedFileId) : IRequest;