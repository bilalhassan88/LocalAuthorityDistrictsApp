using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.API.Controllers
{
    [ApiController]
    [Route("api/districts")]
    public class DistrictsController : ControllerBase
    {
        private readonly IGeoJsonService _geoJsonService;

        public DistrictsController(IGeoJsonService geoJsonService)
        {
            _geoJsonService = geoJsonService;
        }

        [HttpGet]
        public async IAsyncEnumerable<Feature> GetAllDistricts()
        {
            await foreach (var feature in _geoJsonService.GetAllDistrictsAsync())
            {
                yield return feature;
            }
        }

        [HttpGet("filter")]
        public async IAsyncEnumerable<Feature> FilterByName([FromQuery] string name)
        {
            await foreach (var feature in _geoJsonService.FilterByNameAsync(name))
            {
                yield return feature;
            }
        }
    }
}