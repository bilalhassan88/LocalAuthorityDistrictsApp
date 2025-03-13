using LocalAuthorityDistricts.Domain;

namespace LocalAuthorityDistricts.Application
{
    public interface IGeoJsonService
    {
        Task<List<Feature>> GetAllDistrictsAsync();

        Task<List<Feature>> FilterByNameAsync(string name);
    }
}
