using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetflixCatalog_DioAz204.Func.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<ContainerStorageRepository>();

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
