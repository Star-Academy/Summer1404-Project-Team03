namespace etl_backend.DbConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public sealed class EtlDbContextDesignFactory : IDesignTimeDbContextFactory<EtlDbContext>
{
    public EtlDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = cfg.GetConnectionString("DefaultConnection")
                 ?? "Host=localhost;Port=5432;Database=etl_dev;Username=etl_user;Password=etl_password";

        var opts = new DbContextOptionsBuilder<EtlDbContext>()
            .UseNpgsql(cs)
            .Options;

        return new EtlDbContext(opts);
    }
}
