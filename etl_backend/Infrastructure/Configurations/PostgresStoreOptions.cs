namespace Infrastructure.Configurations;

public sealed class PostgresStoreOptions
{
    public string DefaultSchema { get; set; } = "public";
}