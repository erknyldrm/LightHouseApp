using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using LightHouseData;
using LightHouseInfrastructure;
using LightHouseInfrastructure.Storage;

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

//builder.Services.AddData(); 
builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddLightHouseDataServices(builder.Configuration);
builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("Minio"));
builder.Services.AddHttpClient();


var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
