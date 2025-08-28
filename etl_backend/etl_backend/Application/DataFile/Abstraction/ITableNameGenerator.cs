namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableNameGenerator
{
    string Generate(int stagedFileId, string originalFileName);
}