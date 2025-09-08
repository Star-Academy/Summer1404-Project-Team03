namespace Infrastructure.Files.Abstractions;

public interface ITableNameGenerator
{
    string Generate(int stagedFileId, string originalFileName);
}