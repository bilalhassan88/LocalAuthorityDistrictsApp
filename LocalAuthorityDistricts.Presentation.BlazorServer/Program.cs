using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using LocalAuthorityDistricts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IGeoJsonRepository, GeoJsonRepository>();
builder.Services.AddScoped<IGeoJsonService, GeoJsonService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.Configure<GeoJsonFileSettings>(builder.Configuration.GetSection("GeoJsonFileSettings"));
builder.Services.Configure<ConcurrencyChunkSettings>(builder.Configuration.GetSection("ConcurrencyChunkSettings"));

// Register HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5182") // API base URL
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Handle errors in production
    app.UseHsts(); // Enable HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles(); // Serve static files (e.g., CSS, JS)

app.UseRouting();

app.MapBlazorHub(); // Set up SignalR hub for Blazor Server
app.MapFallbackToPage("/_Host"); // Fallback to the _Host.cshtml page

app.Run();