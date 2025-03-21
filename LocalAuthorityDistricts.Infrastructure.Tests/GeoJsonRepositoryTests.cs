using LocalAuthorityDistricts.Domain;
using LocalAuthorityDistricts.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using FluentAssertions;

namespace InfrastructureTests
{
    public class GeoJsonRepositoryTests 
    {
        private readonly string _testFilePath;
        private readonly Mock<IOptions<GeoJsonFileSettings>> _fileSettingsMock;
        private readonly Mock<ILogger<GeoJsonRepository>> _loggerMock;
        private readonly GeoJsonRepository _repository;

        public GeoJsonRepositoryTests()
        {
            _testFilePath = Path.GetTempFileName();
            _fileSettingsMock = new Mock<IOptions<GeoJsonFileSettings>>();
            _loggerMock = new Mock<ILogger<GeoJsonRepository>>();

            _fileSettingsMock.Setup(s => s.Value).Returns(new GeoJsonFileSettings { FilePath = _testFilePath });

            _repository = new GeoJsonRepository(_fileSettingsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllFeaturesAsync_ShouldReturnEmpty_WhenFileHasNoFeatures()
        {
            var emptyJson = "{\"type\":\"FeatureCollection\",\"features\":[]}";
            await File.WriteAllTextAsync(_testFilePath, emptyJson);

            var result = await CollectAsync(_repository.GetAllFeaturesAsync());

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllFeaturesAsync_ShouldReturnFeatures_WhenFileHasValidData()
        {
            var feature = new Feature(
                GeoJsonObjectType.Feature,
                new Geometry(GeoJsonObjectType.Point, new PointCoordinates(new List<double> { 0.0, 0.0 })),
                new DistrictProperties("CityA", "CA1", 500000, "RegionX")
            );

            var featureCollection = new FeatureCollection { Features = new List<Feature> { feature } };
            var json = JsonSerializer.Serialize(featureCollection, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await File.WriteAllTextAsync(_testFilePath, json);

            var result = await CollectAsync(_repository.GetAllFeaturesAsync());

            result.Should().HaveCount(1);
            result[0].Properties.Name.Should().Be("CityA");
        }

        private static async Task<List<T>> CollectAsync<T>(IAsyncEnumerable<T> source)
        {
            var list = new List<T>();
            await foreach (var item in source)
            {
                list.Add(item);
            }
            return list;
        }
    }
}