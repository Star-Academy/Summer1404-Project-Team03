namespace etl_backend.Domain.Enums;

public enum ProcessingStage
{
    None = 0,
    Uploaded,        // file stored on disk
    SchemaInferred,  // schema+columns persisted, linked
    TableCreated,    
    Loaded           // rows inserted
}