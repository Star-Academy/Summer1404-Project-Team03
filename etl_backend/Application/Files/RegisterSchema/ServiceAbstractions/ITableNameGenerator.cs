namespace Infrastructure.Files.Abstractions;

public interface ITableNameGenerator
{
    string Generate(Guid stagedFileId, string originalFileName);
}