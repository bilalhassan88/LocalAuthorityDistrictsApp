using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class CityService : ICityService
{
    private readonly IGeoJsonRepository _geoJsonRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CityService> _logger;
    private const string CitiesCacheKey = CacheKeys.Cities;
    private static readonly SemaphoreSlim _cacheLock = new(1, 1); // Prevents concurrent cache updates

    public CityService(IGeoJsonRepository geoJsonRepository, IMemoryCache memoryCache, ILogger<CityService> logger)
    {
        _geoJsonRepository = geoJsonRepository;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<List<City>> GetCitiesAsync(string searchQuery = "")
    {
        if (_memoryCache.TryGetValue(CitiesCacheKey, out List<City> cachedCities))
        {
            return FilterCities(cachedCities, searchQuery);
        }

        await _cacheLock.WaitAsync();
        try
        {
            // Double-check after acquiring lock
            if (_memoryCache.TryGetValue(CitiesCacheKey, out cachedCities))
            {
                return FilterCities(cachedCities, searchQuery);
            }

            var cities = new List<City>();

            try
            {
                await foreach (var feature in _geoJsonRepository.GetAllFeaturesAsync().ConfigureAwait(false))
                {
                    if (!string.IsNullOrWhiteSpace(feature.Properties?.Name))
                    {
                        cities.Add(new City { Name = feature.Properties.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching GeoJSON data from repository.");
                return new List<City>(); 
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            _memoryCache.Set(CitiesCacheKey, cities, cacheEntryOptions);
            return FilterCities(cities, searchQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching cities.");
            return new List<City>(); 
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    private static List<City> FilterCities(List<City> cities, string searchQuery)
    {
        return string.IsNullOrEmpty(searchQuery)
            ? cities
            : cities.Where(c => c.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
