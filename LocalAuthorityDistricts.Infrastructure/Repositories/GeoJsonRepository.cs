using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.Infrastructure
{
    public class GeoJsonRepository : IGeoJsonRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public GeoJsonRepository(IOptions<GeoJsonFileSettings> fileSettings)
        {
            _filePath = fileSettings.Value.FilePath;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public async IAsyncEnumerable<Feature> GetAllFeaturesAsync()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"GeoJSON file not found at: {_filePath}");
            }

            using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

            string json = await reader.ReadToEndAsync(); // Read full JSON file
            var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, _options);

            if (featureCollection?.Features == null)
            {
                yield break; // No features found, exit gracefully
            }

            foreach (var feature in featureCollection.Features)
            {
                yield return feature;
                await Task.Yield(); // Allow async execution
            }
        }
    }
}