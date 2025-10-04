using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using LightHouseInfrastructure;


var builder = WebApplication.CreateBuilder(args);


static IEdmModel GetEdmModel()
{
    var odataBuilder = new Microsoft.OData.ModelBuilder.ODataConventionModelBuilder();
    odataBuilder.EntitySet<LightHouseApplication.Dtos.QueryableLightHouseDto>("LightHouses");
    return odataBuilder.GetEdmModel();
}


// Add services to the container.
builder.Services.AddControllers().AddOData(opt =>
    opt.Select()
    .Filter()
    .Expand()
    .Count()
    .SetMaxTop(100)
    .AddRouteComponents("odata", GetEdmModel())
);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .WithSecretVault();
   // .Build();


//builder.Services.AddLightHouseDataServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
