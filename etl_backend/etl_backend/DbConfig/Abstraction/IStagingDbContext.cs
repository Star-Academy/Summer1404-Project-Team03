using etl_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace etl_backend.DbConfig.Abstraction;

public interface IStagingDbContext : ICommonDbContext
{
    DbSet<StagedFile> StagedFiles { get; set; }
}