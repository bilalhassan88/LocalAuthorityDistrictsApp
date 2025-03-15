using LocalAuthorityDistricts.Domain;

namespace LocalAuthorityDistricts.Application
{
    public interface ICityService
    {
        Task<List<City>> GetCitiesAsync(string searchQuery = "");
    }
}