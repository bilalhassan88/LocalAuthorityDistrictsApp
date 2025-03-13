using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAuthorityDistricts.Infrastructure
{
    public class GeoJsonRepository : IGeoJsonRepository
    {
        private readonly string _filePath;

        public GeoJsonRepository(IOptions<GeoJsonFileSettings> fileSettings)
        {
            _filePath = fileSettings.Value.FilePath;
        }

        public async Task<List<Feature>> GetAllFeaturesAsync()
        {
            var geoJsonText = await File.ReadAllTextAsync(_filePath);

            var geoJson = JsonSerializer.Deserialize<GeoJson>(
                geoJsonText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return geoJson?.Features ?? new List<Feature>();
        }
    }
}
