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

        public async Task<List<Feature>> GetAllFeaturesAsync()
        {
            var geoJsonContent = await File.ReadAllTextAsync(_filePath);
            var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(geoJsonContent, _options);
            return featureCollection?.Features ?? new List<Feature>();
        }
    }
}