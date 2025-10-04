
using LightHouseApplication;
using LightHouseData;
using LightHouseDomain.Interfaces;
using LightHouseInfrastructure;
using LightHouseInfrastructure.Auditors;
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
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration)
.WithSecretVault()
.WithPhotoStorage()
.WithCaching()
.WithExternals();

builder.Services.AddLightHouseDataServices();

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
