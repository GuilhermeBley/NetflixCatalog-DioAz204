using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixCatalog_DioAz204.Func;
using NetflixCatalog_DioAz204.Func.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration
            .AddEnvironmentVariables()
            .AddJsonFile("local.settings.json", optional: true)
            .AddUserSecrets(typeof(Program).Assembly);

builder.Services.AddSingleton<ContainerStorageRepository>();
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddDbContext<AppDbContext>();

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<StorageAccountOptions>(o =>
    {
        o.Container = builder.Configuration["StorageAccount:Container"]?.ToString() ?? string.Empty;
        o.ConnectionString = builder.Configuration["StorageAccount:ConnectionString"]?.ToString() ?? string.Empty;
    });

builder.Services.Configure<CosmosDbOption>(o =>
    {
        o.AccountKey = builder.Configuration["Context:AccountKey"]?.ToString() ?? string.Empty;
        o.Endpoint = builder.Configuration["Context:Endpoint"]?.ToString() ?? string.Empty;
        o.DataBaseName = builder.Configuration["Context:DataBaseName"]?.ToString() ?? string.Empty;
        o.ConnectionString = builder.Configuration["Context:ConnectionString"]?.ToString() ?? string.Empty;
        o.Container = builder.Configuration["Context:Container"]?.ToString() ?? string.Empty;
    });

var build = builder.Build();

// Requesting for the context to execute the async seeding
build.Services.GetRequiredService<AppDbContext>();

build.Run();