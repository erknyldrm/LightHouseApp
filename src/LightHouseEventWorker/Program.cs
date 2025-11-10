using LightHouseEventWorker;
using LightHouseInfrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration)
.WithSecretVault()
.WithMessaging()
.Build();

//builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
