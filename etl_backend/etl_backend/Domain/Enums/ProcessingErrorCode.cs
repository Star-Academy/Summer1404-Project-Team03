namespace etl_backend.Domain.Enums;

public enum ProcessingErrorCode
{
    None = 0,
    StorageSaveFailed,
    StagingDbWriteFailed,
    SchemaRegistered,
    SchemaDbWriteFailed,
    CreateTableFailed,
    LoadFailed,
    ValidationFailed
}