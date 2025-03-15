using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CityService : ICityService
{
    private readonly IGeoJsonRepository _geoJsonRepository;
    private readonly IMemoryCache _memoryCache;
    private const string CitiesCacheKey = "CitiesCacheKey";

    public CityService(IGeoJsonRepository geoJsonRepository, IMemoryCache memoryCache)
    {
        _geoJsonRepository = geoJsonRepository;
        _memoryCache = memoryCache;
    }

    public async Task<List<City>> GetCitiesAsync(string searchQuery = "")
    {
        // Try to get the cities from the cache.
        if (!_memoryCache.TryGetValue(CitiesCacheKey, out List<City> cities))
        {
            cities = new List<City>();

            // Stream data from the repository.
            await foreach (var feature in _geoJsonRepository.GetAllFeaturesAsync())
            {
                if (feature.Properties != null)
                {
                    cities.Add(new City { Name = feature.Properties.Name });
                }
            }

            // Set cache options for one hour expiration.
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            // Save the cities list in the cache.
            _memoryCache.Set(CitiesCacheKey, cities, cacheEntryOptions);
        }

        // Filter based on the search query if provided.
        var filteredCities = cities
            .Where(c => string.IsNullOrEmpty(searchQuery) ||
                        c.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return filteredCities;
    }
}
