using LocalAuthorityDistricts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.Application
{
    public interface IGeoJsonService
    {
        IAsyncEnumerable<Feature> GetAllDistrictsAsync();
        IAsyncEnumerable<Feature> FilterByNameAsync(string name);
    }
}