using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbConfig.Abstraction;

public interface IStagingDbContext : ICommonDbContext
{
    DbSet<StagedFile> StagedFiles { get; set; }
}