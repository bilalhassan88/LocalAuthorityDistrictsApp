using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalAuthorityDistricts.Infrastructure
{
    public class GeoJsonRepository : IGeoJsonRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;
        private readonly ILogger<GeoJsonRepository> _logger;

        public GeoJsonRepository(IOptions<GeoJsonFileSettings> fileSettings, ILogger<GeoJsonRepository> logger)
        {
            _filePath = fileSettings.Value.FilePath;
            _logger = logger;
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
                _logger.LogError("GeoJSON file not found at: {FilePath}", _filePath);
                yield break;
            }

            FeatureCollection? featureCollection = null;

            try
            {
                using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

                string json = await reader.ReadToEndAsync();
                featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, _options);

                if (featureCollection?.Features == null)
                {
                    _logger.LogWarning("GeoJSON file at {FilePath} contains no features.", _filePath);
                    yield break;
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize GeoJSON file at {FilePath}", _filePath);
                yield break;
            }
            catch (IOException ioEx)
            {
                _logger.LogError(ioEx, "I/O error while reading the GeoJSON file at {FilePath}", _filePath);
                yield break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing GeoJSON file at {FilePath}", _filePath);
                yield break;
            }

            // Now iterate safely outside of try-catch
            foreach (var feature in featureCollection.Features)
            {
                yield return feature;
                await Task.Yield();
            }
        }
    }
}
