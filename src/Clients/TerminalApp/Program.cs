using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;
using LightHouseDomain.Entities;
using LightHouseInfrastructure.Storage;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole());

var connectionString = config.GetConnectionString("lighthousedb");
LightHouseData.DependencyInjection.AddLightHouseDataServices(services, connectionString);
LightHouseApplication.DependencyInjection.AddApplicationServices(services);
LightHouseInfrastructure.DependencyInjection.AddInfrastructureServices(services);
services.Configure<MinioSettings>(config.GetSection("Minio"));  

var serviceProvider = services.BuildServiceProvider();
var lightHouseService = serviceProvider.GetRequiredService<ILightHouseService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

var newLighthouse = await lightHouseService.CreateLightHouseAsync(new LightHouseDto(Guid.NewGuid(), "My First Lighthouse", 5, 34.0522, -118.2437));

Console.WriteLine($"Created Lighthouse Id: {newLighthouse}");
logger.LogInformation("Created a new lighthouse with ID: {LighthouseId}", newLighthouse);

var lighthouses = await lightHouseService.GetLightHousesAsync();

logger.LogInformation("Retrieved {Count} lighthouses from the database.", lighthouses.Count());
Console.WriteLine($"Total Lighthouses: {lighthouses.Count()}");

var photoService = serviceProvider.GetRequiredService<IPhotoService>();

var file = File.OpenRead("sample.jpg");

var created = photoService.UploadPhotoAsync(new PhotoDto
(
    Guid.NewGuid(), "cape.jpg", DateTime.UtcNow, "Canon EOS 5D Mark IV", Guid.NewGuid(), newLighthouse
), file).ContinueWith(t =>
{
    if (t.IsCompletedSuccessfully)
    {
        Console.WriteLine($"Uploaded photo with ID: {t.Result}");
    }
    else
    {
        logger.LogError(t.Exception, "Failed to upload photo.");
    }
});

