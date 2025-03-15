using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace LocalAuthorityDistricts.Application
{
    public class GeoJsonService : IGeoJsonService
    {
        private readonly IGeoJsonRepository _repository;
        private readonly ConcurrencyChunkSettings _chunkSettings;
        private readonly IMemoryCache _memoryCache;
        private const string AllDistrictsCacheKey = CacheKeys.AllDistricts;

        public GeoJsonService(
            IGeoJsonRepository repository,
            IOptions<ConcurrencyChunkSettings> chunkSettings,
            IMemoryCache memoryCache)
        {
            _repository = repository;
            _chunkSettings = chunkSettings.Value;
            _memoryCache = memoryCache;
        }

        public async IAsyncEnumerable<Feature> GetAllDistrictsAsync()
        {
            if (_memoryCache.TryGetValue(AllDistrictsCacheKey, out List<Feature>? cachedFeatures))
            {
                foreach (var feature in cachedFeatures)
                {
                    yield return feature;
                    await Task.Yield();
                }
                yield break;
            }

            var features = new List<Feature>();
            bool fullyRetrieved = false;

            try
            {
                await foreach (var feature in _repository.GetAllFeaturesAsync())
                {
                    features.Add(feature);
                }

                fullyRetrieved = true; 
            }
            catch
            {
                fullyRetrieved = false;
                throw;
            }

            if (fullyRetrieved && features.Count > 0)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };

                _memoryCache.Set(AllDistrictsCacheKey, features, cacheEntryOptions);
            }

            foreach (var feature in features)
            {
                yield return feature;
                await Task.Yield();
            }
        }

        public async IAsyncEnumerable<Feature> FilterByNameAsync(IEnumerable<string> names)
        {
            if (_memoryCache.TryGetValue(AllDistrictsCacheKey, out List<Feature>? allFeatures))
            {
                foreach (var feature in allFeatures.Where(f => names.Any(name =>
                         f.Properties.Name.Contains(name, StringComparison.OrdinalIgnoreCase))))
                {
                    yield return feature;
                    await Task.Yield();
                }
                yield break;
            }

            var batch = new List<Feature>(_chunkSettings.ChunkSize);

            await foreach (var feature in _repository.GetAllFeaturesAsync())
            {
                if (names.Any(name => feature.Properties.Name.Contains(name, StringComparison.OrdinalIgnoreCase)))
                {
                    batch.Add(feature);
                }

                if (batch.Count >= _chunkSettings.ChunkSize)
                {
                    var matchingFeatures = new ConcurrentBag<Feature>();

                    await Parallel.ForEachAsync(batch, async (feature, ct) =>
                    {
                        matchingFeatures.Add(feature);
                        await Task.Yield();
                    });

                    foreach (var matchingFeature in matchingFeatures)
                    {
                        yield return matchingFeature;
                        await Task.Yield();
                    }

                    batch.Clear();
                }
            }

            foreach (var feature in batch)
            {
                yield return feature;
                await Task.Yield();
            }
        }
    }
}
