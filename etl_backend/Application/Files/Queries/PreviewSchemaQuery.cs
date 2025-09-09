using MediatR;

namespace Application.Files.Queries;

public record PreviewSchemaQuery(int StagedFileId) : IRequest<ColumnPreviewResponse>;

public record ColumnPreviewResponse(
    int StagedFileId,
    List<ColumnPreviewItem> Columns
);

public record ColumnPreviewItem(
    int OrdinalPosition,
    string ColumnName,
    string OriginalColumnName, 
    string ColumnType
);