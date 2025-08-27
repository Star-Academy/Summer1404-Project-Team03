using etl_backend.Domain;
using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IDataFileStageable
{
    Task<DataTableSchema> StageFile(Stream fileStream);    
}