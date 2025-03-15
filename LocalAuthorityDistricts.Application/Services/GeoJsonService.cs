using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.Application
{
    public class GeoJsonService : IGeoJsonService
    {
        private readonly IGeoJsonRepository _repository;
        private readonly ConcurrencyChunkSettings _chunkSettings;

        public GeoJsonService(
            IGeoJsonRepository repository,
            IOptions<ConcurrencyChunkSettings> chunkSettings)
        {
            _repository = repository;
            _chunkSettings = chunkSettings.Value;
        }

        public async IAsyncEnumerable<Feature> GetAllDistrictsAsync()
        {
            var batch = new List<Feature>(_chunkSettings.ChunkSize);

            await foreach (var feature in _repository.GetAllFeaturesAsync())
            {
                batch.Add(feature);

                if (batch.Count >= _chunkSettings.ChunkSize)
                {
                    var processedFeatures = new ConcurrentBag<Feature>();

                    await Parallel.ForEachAsync(batch, async (feature, ct) =>
                    {
                        processedFeatures.Add(feature);
                        await Task.Yield(); // Ensures true async execution
                    });

                    foreach (var processedFeature in processedFeatures)
                    {
                        yield return processedFeature;
                        await Task.Yield(); // Async-friendly yielding
                    }

                    batch.Clear();
                }
            }

            // Yield remaining items
            foreach (var feature in batch)
            {
                yield return feature;
                await Task.Yield();
            }
        }

        public async IAsyncEnumerable<Feature> FilterByNameAsync(IEnumerable<string> names)
        {
            var batch = new List<Feature>(_chunkSettings.ChunkSize);

            await foreach (var feature in _repository.GetAllFeaturesAsync())
            {
                if (names.Any(name => feature.Properties.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase)))
                {
                    batch.Add(feature);
                }

                if (batch.Count >= _chunkSettings.ChunkSize)
                {
                    var matchingFeatures = new ConcurrentBag<Feature>();

                    await Parallel.ForEachAsync(batch, async (feature, ct) =>
                    {
                        matchingFeatures.Add(feature);
                        await Task.Yield(); // Allows async processing
                    });

                    foreach (var matchingFeature in matchingFeatures)
                    {
                        yield return matchingFeature;
                        await Task.Yield(); // Ensures proper yielding
                    }

                    batch.Clear();
                }
            }

            // Yield remaining items
            foreach (var feature in batch)
            {
                yield return feature;
                await Task.Yield();
            }
        }
    }
}
