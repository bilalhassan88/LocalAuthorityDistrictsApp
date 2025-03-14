﻿using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public async IAsyncEnumerable<Feature> FilterByNameAsync(string name)
        {
            var allFeatures = await _repository.GetAllFeaturesAsync();
            var batches = allFeatures.Chunk(_chunkSettings.ChunkSize);

            foreach (var batch in batches)
            {
                foreach (var feature in batch)
                {
                    if (feature.Properties.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        yield return feature;
                    }
                }
            }
        }
    }
}