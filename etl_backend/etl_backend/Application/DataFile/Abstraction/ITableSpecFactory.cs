using etl_backend.Application.DataFile.Dtos;
using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableSpecFactory
{
    TableSpec From(DataTableSchema schema);
}