using LightHouseEventWorker;
using LightHouseEventWorker.EventHandlers;
using LightHouseEventWorker.Services;
using LightHouseInfrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration)
.WithCaching()
.WithSecretVault()
.WithMessaging()
.Build();

builder.Services.AddScoped<IPhotoUploadedEventHandler, PhotoUploadedEventHandler>();    

builder.Services.AddHostedService<RabbitMqEventConsumerService>();

var host = builder.Build();
await host.RunAsync();
