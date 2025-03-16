using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace ApplicationTests
{
    public class CityServiceTests
    {
        private readonly Mock<IGeoJsonRepository> _repositoryMock;
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<ILogger<CityService>> _loggerMock;
        private readonly CityService _service;

        public CityServiceTests()
        {
            _repositoryMock = new Mock<IGeoJsonRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _loggerMock = new Mock<ILogger<CityService>>();

            _service = new CityService(_repositoryMock.Object, _memoryCache, _loggerMock.Object);
        }

        [Fact]
        public async Task GetCitiesAsync_ShouldReturnCitiesFromRepository_WhenCacheIsEmpty()
        {
            var features = new List<Feature>
            {
                CreateFeature("CityA", "CA1", 100000, "Region1"),
                CreateFeature("CityB", "CB1", 200000, "Region2")
            };

            _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

            var result = await _service.GetCitiesAsync();

            result.Should().HaveCount(2);
            result.Select(c => c.Name).Should().BeEquivalentTo(new[] { "CityA", "CityB" });
            _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCitiesAsync_ShouldReturnCitiesFromCache_WhenCacheIsPopulated()
        {
            var cachedCities = new List<City>
            {
                new City { Name = "CachedCityA" },
                new City { Name = "CachedCityB" }
            };
            _memoryCache.Set(CacheKeys.Cities, cachedCities, TimeSpan.FromHours(1));

            var result = await _service.GetCitiesAsync();

            result.Should().BeEquivalentTo(cachedCities);
            _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetCitiesAsync_ShouldFilterCitiesBySearchQuery()
        {
            var features = new List<Feature>
            {
                CreateFeature("New York", "NY1", 8000000, "East Coast"),
                CreateFeature("Los Angeles", "LA1", 4000000, "West Coast"),
                CreateFeature("New Orleans", "NO1", 390000, "South")
            };

            _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

            var result = await _service.GetCitiesAsync("New");

            result.Should().HaveCount(2);
            result.Select(c => c.Name).Should().BeEquivalentTo(new[] { "New York", "New Orleans" });
        }

        [Fact]
        public async Task GetCitiesAsync_ShouldReturnEmptyList_WhenNoMatchesFound()
        {
            var features = new List<Feature>
            {
                CreateFeature("CityA", "CA1", 500000, "RegionX"),
                CreateFeature("CityB", "CB1", 600000, "RegionY")
            };

            _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

            var result = await _service.GetCitiesAsync("Zzz");

            result.Should().BeEmpty();
        }

        private static Feature CreateFeature(string name, string code, int population, string region)
        {
            return new Feature(
                GeoJsonObjectType.Feature,
                new Geometry(GeoJsonObjectType.Point, new PointCoordinates(new List<double> { 0.0, 0.0 })),
                new DistrictProperties(name, code, population, region)
            );
        }

        private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                yield return item;
                await Task.Yield();
            }
        }
    }
}
