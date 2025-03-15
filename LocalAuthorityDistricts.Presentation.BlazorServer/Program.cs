using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using LocalAuthorityDistricts.Infrastructure;
using LocalAuthorityDistricts.Presentation.BlazorServer.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IGeoJsonRepository, GeoJsonRepository>();
builder.Services.AddScoped<IGeoJsonService, GeoJsonService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.Configure<GeoJsonFileSettings>(builder.Configuration.GetSection("GeoJsonFileSettings"));
builder.Services.Configure<ChunkSettings>(builder.Configuration.GetSection("ChunkSettings"));
builder.Services.Configure<MapboxSettings>(builder.Configuration.GetSection("MapboxSettings"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); 
    app.UseHsts(); 
}

app.UseHttpsRedirection(); 
app.UseStaticFiles(); 

app.UseRouting();

app.MapBlazorHub(); 
app.MapFallbackToPage("/_Host"); 

app.Run();