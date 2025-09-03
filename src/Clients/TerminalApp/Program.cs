using LightHouseApplication.Contracts;
using LightHouseApplication.Dtos;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole());

var connectionString = config.GetConnectionString("lighthousedb");
LightHouseData.DependencyInjection.AddLightHouseDataServices(services, connectionString);
LightHouseApplication.DependencyInjection.AddApplicationServices(services);

var serviceProvider = services.BuildServiceProvider();  
var lightHouseService = serviceProvider.GetRequiredService<ILightHouseService>();
var newLighthouse = await lightHouseService.CreateLightHouseAsync(new LightHouseDto(Guid.NewGuid(), "My First Lighthouse", 5, 34.0522, -118.2437));

Console.WriteLine($"Created Lighthouse Id: {newLighthouse}");

var lighthouses = await lightHouseService.GetLightHousesAsync();

Console.WriteLine($"Total Lighthouses: {lighthouses.Count()}");