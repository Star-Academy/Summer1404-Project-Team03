namespace WebApi.Files;

public class StageManyFilesResponseItem
{
    public string FileName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? Error { get; set; }
    public StageFileResponse? Data { get; set; }
}

public class StageFileResponse
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoredFilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Stage { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}