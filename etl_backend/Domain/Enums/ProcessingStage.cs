namespace Domain.Enums;

public enum ProcessingStage
{
    None = 0,
    Uploaded,        // file stored on disk
    SchemaRegistered,  // schema+columns persisted, linked
    TableCreated,    
    Loaded           // rows inserted
}