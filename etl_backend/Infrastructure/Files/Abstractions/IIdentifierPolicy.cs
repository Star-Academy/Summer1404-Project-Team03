namespace Infrastructure.Files.Abstractions;

public interface IIdentifierPolicy
{
    string DefaultSchema { get; }
    string QuoteIdentifier(string identifier);
    string QualifyTable(string? schema, string table); // returns fully quoted
}