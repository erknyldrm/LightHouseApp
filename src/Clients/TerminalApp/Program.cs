using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseDomain.Entities;
using LightHouseInfrastructure.Configuration;
using LightHouseInfrastructure.Storage;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole());


LightHouseApplication.DependencyInjection.AddApplicationServices(services);
LightHouseInfrastructure.DependencyInjection.AddInfrastructureServices(services, config);


LightHouseData.DependencyInjection.AddLightHouseDataServices(services, provider =>
{
    var vault = provider.GetRequiredService<VaultConfigurationService>();

    try
    {
        return vault.GetDatabaseConnectionStringAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = provider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to retrieve the database connection string from Vault.");
        return String.Empty;
    }
});

services.Configure<MinioSettings>(config.GetSection("Minio"));

services.AddSingleton<IConfiguration>();
var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Created");

