using Domain.Entities;
using Infrastructure.Files.Dtos;

namespace Infrastructure.Files.Abstractions;

public interface ITableSpecFactory
{
    TableSpec From(DataTableSchema schema);
}