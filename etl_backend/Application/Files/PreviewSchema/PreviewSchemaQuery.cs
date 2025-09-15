using Application.Common.Authorization;
using MediatR;

namespace Application.Files.Queries;
[RequireRole(AppRoles.Analyst, AppRoles.DataAdmin, AppRoles.SysAdmin)]
public record PreviewSchemaQuery(Guid StagedFileId) : IRequest<ColumnPreviewResponse>;

public record ColumnPreviewResponse(
    Guid StagedFileId,
    List<ColumnPreviewItem> Columns
);

public record ColumnPreviewItem(
    int OrdinalPosition,
    string ColumnName,
    string OriginalColumnName, 
    string ColumnType
);