using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;

public class CityService : ICityService
{
    private readonly IGeoJsonRepository _geoJsonRepository;
    private readonly IMemoryCache _memoryCache;
    private const string CitiesCacheKey = CacheKeys.Cities;

    public CityService(IGeoJsonRepository geoJsonRepository, IMemoryCache memoryCache)
    {
        _geoJsonRepository = geoJsonRepository;
        _memoryCache = memoryCache;
    }

    public async Task<List<City>> GetCitiesAsync(string searchQuery = "")
    {
        if (!_memoryCache.TryGetValue(CitiesCacheKey, out List<City> cities))
        {
            cities = new List<City>();

            await foreach (var feature in _geoJsonRepository.GetAllFeaturesAsync())
            {
                if (feature.Properties != null)
                {
                    cities.Add(new City { Name = feature.Properties.Name });
                }
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            _memoryCache.Set(CitiesCacheKey, cities, cacheEntryOptions);
        }

        var filteredCities = cities
            .Where(c => string.IsNullOrEmpty(searchQuery) ||
                        c.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return filteredCities;
    }
}
