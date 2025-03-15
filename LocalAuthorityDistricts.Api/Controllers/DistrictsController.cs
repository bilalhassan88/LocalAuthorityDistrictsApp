using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class DistrictsController : ControllerBase
{
    private readonly IGeoJsonService _geoJsonService;
    private readonly ILogger<DistrictsController> _logger;

    public DistrictsController(IGeoJsonService geoJsonService, ILogger<DistrictsController> logger)
    {
        _geoJsonService = geoJsonService;
        _logger = logger;
    }

    [HttpGet("all")]
    public async IAsyncEnumerable<Feature> GetAllDistricts()
    {
        await foreach (var feature in _geoJsonService.GetAllDistrictsAsync())
        {
            _logger.LogInformation("Yielding feature: {FeatureName}", feature.Properties.Name);
            yield return feature;
        }
    }



}