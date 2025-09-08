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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200", "http://localhost:7252", "https://localhost:7252",
                "http://192.168.25.195:4200", "https://192.168.25.178:7252"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            ;
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
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