
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure.Auditors;
using LightHouseInfrastructure.Configuration;
using LightHouseInfrastructure.SecretManager;
using LightHouseInfrastructure.Storage;


var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
LightHouseApplication.DependencyInjection.AddApplicationServices(builder.Services);
LightHouseInfrastructure.DependencyInjection.AddInfrastructureServices(builder.Services, config);

LightHouseData.DependencyInjection.AddLightHouseDataServices(builder.Services, provider =>
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


builder.Services.Configure<MinioSettings>(config.GetSection("Minio"));
builder.Services.AddHttpClient<ICommentAuditor, ExternalCommentAuditor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
