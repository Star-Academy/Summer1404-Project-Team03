using Domain.Enums;

namespace Domain.Entities;


public class StagedFile
{
    public int Id { get; set; }
    
    
    public required string OriginalFileName { get; set; }
    
    public required string StoredFilePath { get; set; }
    public long FileSize { get; set; }
    
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    public ProcessingStage Stage { get; set; } = ProcessingStage.None;
    public ProcessingStatus Status { get; set; } = ProcessingStatus.InProgress;
    public ProcessingErrorCode ErrorCode { get; set; } = ProcessingErrorCode.None;
    
    
    public string? ErrorMessage { get; set; } // in case staging/validation fails
    
    public int? SchemaId { get; set; }
    
    public DataTableSchema? Schema { get; set; }
}