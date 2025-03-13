using LocalAuthorityDistricts.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.API
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

        // GET: api/districts/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDistricts()
        {
            var features = await _geoJsonService.GetAllDistrictsAsync();
            return Ok(features);
        }

        // GET: api/districts/filter?name=Oxford
        [HttpGet("filter")]
        public async Task<IActionResult> FilterByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var all = await _geoJsonService.GetAllDistrictsAsync();
                return Ok(all);
            }

            var matches = await _geoJsonService.FilterByNameAsync(name);
            return Ok(matches);
        }
    }
}
