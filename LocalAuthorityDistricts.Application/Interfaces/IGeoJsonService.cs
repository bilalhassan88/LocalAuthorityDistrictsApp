using LocalAuthorityDistricts.Domain;

namespace LocalAuthorityDistricts.Application
{
    public interface IGeoJsonService
    {
        IAsyncEnumerable<Feature> GetAllDistrictsAsync();
        IAsyncEnumerable<Feature> FilterByNameAsync(IEnumerable<string> names);

    }
}