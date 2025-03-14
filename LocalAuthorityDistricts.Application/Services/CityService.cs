using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CityService : ICityService
{
    private readonly IGeoJsonRepository _geoJsonRepository;

    public CityService(IGeoJsonRepository geoJsonRepository)
    {
        _geoJsonRepository = geoJsonRepository;
    }

    public async Task<List<City>> GetCitiesAsync(int pageNumber, int pageSize, string searchQuery = "")
    {
        var features = await _geoJsonRepository.GetAllFeaturesAsync();

        var cities = features
            .Where(f => f.Properties != null)
            .Select(f => new City
            {
                Name = f.Properties.Name
            })
            .ToList();

        var filteredCities = cities
            .Where(c => string.IsNullOrEmpty(searchQuery) ||
                        c.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var paginatedCities = filteredCities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return paginatedCities;
    }
}