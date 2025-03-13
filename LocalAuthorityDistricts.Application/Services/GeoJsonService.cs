using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;


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

        public async Task<List<Feature>> GetAllDistrictsAsync()
        {
            var allFeatures = await _repository.GetAllFeaturesAsync();

            var batches = allFeatures.Chunk(_chunkSettings.ChunkSize);
            var bag = new ConcurrentBag<Feature>();

            await Parallel.ForEachAsync(batches, async (batch, ct) =>
            {
                // optional: simulate "work"
                await Task.Delay(50, ct);
                foreach (var f in batch)
                {
                    bag.Add(f);
                }
            });

            return bag.ToList();
        }

        public async Task<List<Feature>> FilterByNameAsync(string name)
        {
            var allFeatures = await _repository.GetAllFeaturesAsync();

            var chunked = allFeatures.Chunk(_chunkSettings.ChunkSize);
            var bag = new ConcurrentBag<Feature>();

            await Parallel.ForEachAsync(chunked, async (batch, ct) =>
            {
                await Task.Delay(50, ct);
                foreach (var f in batch)
                {
                    if (f.Properties.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        bag.Add(f);
                    }
                }
            });

            return bag.ToList();
        }
    }
}
