using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IColumnDefinitionBuilder
{
    List<DataTableColumn> Build(IReadOnlyList<string> headers);
}