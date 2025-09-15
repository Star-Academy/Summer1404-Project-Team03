using Domain.Entities;

namespace Infrastructure.Files.Abstractions;

public interface IColumnDefinitionBuilder
{
    List<DataTableColumn> Build(IReadOnlyList<string> headers);
}