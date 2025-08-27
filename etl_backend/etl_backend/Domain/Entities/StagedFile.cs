using etl_backend.Domain.Enums;

namespace etl_backend.Domain.Entities;

public class StagedFile
{
    public int Id { get; set; }
    
    
    public required string OriginalFileName { get; set; }
    
    public required string StoredFilePath { get; set; }
    public long FileSize { get; set; }
    
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    public FileStageState State { get; set; }
    
    
    public string? ErrorMessage { get; set; } // in case staging/validation fails
    
    public int? SchemaId { get; set; }
    
    public DataTableSchema? Schema { get; set; }
}