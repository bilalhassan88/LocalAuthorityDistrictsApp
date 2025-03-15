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
            var allFeatures = await _repository.GetAllFeaturesAsync();
            var batches = allFeatures.Chunk(_chunkSettings.ChunkSize);

            foreach (var batch in batches)
            {
                var processedFeatures = new ConcurrentBag<Feature>();

                await Parallel.ForEachAsync(batch, async (feature, ct) =>
                {
                    processedFeatures.Add(feature);
                });

                foreach (var feature in processedFeatures)
                {
                    yield return feature;
                }
            }
        }

        public async IAsyncEnumerable<Feature> FilterByNameAsync(IEnumerable<string> names)
        {
            var allFeatures = await _repository.GetAllFeaturesAsync();
            var batches = allFeatures.Chunk(_chunkSettings.ChunkSize);

            foreach (var batch in batches)
            {
                var matchingFeatures = new ConcurrentBag<Feature>();

                await Parallel.ForEachAsync(batch, async (feature, ct) =>
                {
                    if (names.Any(name => feature.Properties.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        matchingFeatures.Add(feature);
                    }
                });

                foreach (var feature in matchingFeatures)
                {
                    yield return feature;
                }
            }
        }
    }
}