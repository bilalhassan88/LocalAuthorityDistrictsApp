using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using LocalAuthorityDistricts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<GeoJsonFileSettings>(
    builder.Configuration.GetSection("GeoJsonFileSettings"));

builder.Services.Configure<ConcurrencyChunkSettings>(
    builder.Configuration.GetSection("ConcurrencyChunkSettings"));

builder.Services.AddSingleton<IGeoJsonRepository, GeoJsonRepository>();
builder.Services.AddScoped<IGeoJsonService, GeoJsonService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();
