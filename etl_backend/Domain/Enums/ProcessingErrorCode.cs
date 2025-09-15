namespace Domain.Enums;

public enum ProcessingErrorCode
{
    None = 0,
    StorageSaveFailed,
    StagingDbWriteFailed,
    SchemaRegistrationFailed,
    SchemaDbWriteFailed,
    CreateTableFailed,
    LoadFailed,
    ValidationFailed
}