namespace WebApi.Files;

public class ListStagedFilesResponse
{
    public List<ListStagedFilesItemResponse> Items { get; set; } = new();
}

public class ListStagedFilesItemResponse
{
    public Guid Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string Stage { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? SchemaId { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
}