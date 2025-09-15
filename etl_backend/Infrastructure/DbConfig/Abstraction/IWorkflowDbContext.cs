using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbConfig.Abstraction;

public interface IWorkflowDbContext : ICommonDbContext
{
    DbSet<Workflow> Workflows { get; set; }
    DbSet<Plugin> Plugins { get; set; }
}