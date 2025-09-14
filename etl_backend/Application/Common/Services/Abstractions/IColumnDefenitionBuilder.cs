using Domain.Entities;

namespace Application.Abstractions;

public interface IColumnDefinitionBuilder
{
    List<DataTableColumn> Build(IReadOnlyList<string> headers);
}