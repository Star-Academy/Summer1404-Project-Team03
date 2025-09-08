using Application;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.DbConfig;
using Microsoft.EntityFrameworkCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection("Storage"));
builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.ShortSchemaNames = true;
    });
builder.Services.AddSwaggerGen();


// ✅ Add services: Application → Infrastructure → Web
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebServices();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var cs = configuration.GetConnectionString("DefaultConnection");

    var options = new DbContextOptionsBuilder<EtlDbContext>()
        .UseNpgsql(cs)
        .Options;

    await using var db = new EtlDbContext(options);
    var pending = await db.Database.GetPendingMigrationsAsync();
    if (pending.Any())
    {
        Console.WriteLine("Applying migrations: " + string.Join(", ", pending));
        await db.Database.MigrateAsync();
    }
}


app.UseFastEndpoints().UseSwaggerGen();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();