using LightHouseApplication;
using LightHouseData;
using LightHouseInfrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application Services
builder.Services.AddScoped<ILightHouseService, LightHouseService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// Add Infrastructure Services
builder.Services.AddScoped<ICommentAuditor, DefaultCommentAuditor>();
builder.Services.AddScoped<ICommentAuditor, ExternalCommentAuditor>();

// Add Data Services
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILightHouseRepository, LightHouseRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add DbContext
builder.Services.AddDbContext<LightHouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
