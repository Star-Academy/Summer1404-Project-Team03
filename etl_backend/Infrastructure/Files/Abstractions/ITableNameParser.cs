namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableNameParser
{
    (string Schema, string Table) Parse(string name, string defaultSchema);
}