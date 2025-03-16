using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LocalAuthorityDistricts.Application
{
    public class GeoJsonService : IGeoJsonService
    {
        private readonly IGeoJsonRepository _repository;
        private readonly ConcurrencyChunkSettings _chunkSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GeoJsonService> _logger;
        private const string AllDistrictsCacheKey = CacheKeys.AllDistricts;
        private static readonly SemaphoreSlim _cacheLock = new(1, 1);

        public GeoJsonService(
            IGeoJsonRepository repository,
            IOptions<ConcurrencyChunkSettings> chunkSettings,
            IMemoryCache memoryCache,
            ILogger<GeoJsonService> logger)
        {
            _repository = repository;
            _chunkSettings = chunkSettings.Value;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async IAsyncEnumerable<Feature> GetAllDistrictsAsync()
        {
            List<Feature> features = new();

            if (_memoryCache.TryGetValue(AllDistrictsCacheKey, out List<Feature>? cachedFeatures))
            {
                features = cachedFeatures;
            }
            else
            {
                await _cacheLock.WaitAsync();
                try
                {
                    if (!_memoryCache.TryGetValue(AllDistrictsCacheKey, out features))
                    {
                        features = new List<Feature>();

                        try
                        {
                            await foreach (var feature in _repository.GetAllFeaturesAsync().ConfigureAwait(false))
                            {
                                features.Add(feature);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to fetch features from repository.");
                            features.Clear(); 
                        }

                        if (features.Count > 0)
                        {
                            _memoryCache.Set(AllDistrictsCacheKey, features, new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                            });
                        }
                    }
                }
                finally
                {
                    _cacheLock.Release();
                }
            }

            foreach (var feature in features)
            {
                yield return feature;
                await Task.Yield();
            }
        }

        public async IAsyncEnumerable<Feature> FilterByNameAsync(IEnumerable<string> names)
        {
            List<Feature> matchingFeatures = new();

            try
            {
                if (_memoryCache.TryGetValue(AllDistrictsCacheKey, out List<Feature>? allFeatures))
                {
                    matchingFeatures = allFeatures
                        .Where(f => names.Any(name => f.Properties?.Name?.Contains(name, StringComparison.OrdinalIgnoreCase) == true))
                        .ToList();
                }
                else
                {
                    var batch = new List<Feature>(_chunkSettings.ChunkSize);

                    await foreach (var feature in _repository.GetAllFeaturesAsync().ConfigureAwait(false))
                    {
                        if (names.Any(name => feature.Properties?.Name?.Contains(name, StringComparison.OrdinalIgnoreCase) == true))
                        {
                            batch.Add(feature);
                        }

                        if (batch.Count >= _chunkSettings.ChunkSize)
                        {
                            matchingFeatures.AddRange(batch);
                            batch.Clear();
                        }
                    }

                    matchingFeatures.AddRange(batch);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while filtering districts by name.");
                yield break; 
            }

            foreach (var feature in matchingFeatures)
            {
                yield return feature;
                await Task.Yield();
            }
        }
    }
}
