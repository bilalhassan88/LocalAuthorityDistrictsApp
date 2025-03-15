using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Xunit;
using FluentAssertions;

public class GeoJsonServiceTests
{
    private readonly Mock<IGeoJsonRepository> _repositoryMock;
    private readonly IMemoryCache _memoryCache;
    private readonly GeoJsonService _service;
    private readonly ConcurrencyChunkSettings _chunkSettings;

    public GeoJsonServiceTests()
    {
        _repositoryMock = new Mock<IGeoJsonRepository>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _chunkSettings = new ConcurrencyChunkSettings { ChunkSize = 2 };
        var options = Options.Create(_chunkSettings);

        _service = new GeoJsonService(_repositoryMock.Object, options, _memoryCache);
    }

    [Fact]
    public async Task GetAllDistrictsAsync_ShouldReturnFeaturesFromRepository_WhenCacheIsEmpty()
    {
        var features = new List<Feature> { CreateFeature("District1", "D1", 50000, "Region1") };
        _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

        var result = await CollectAsync(_service.GetAllDistrictsAsync());

        result.Should().BeEquivalentTo(features);
        _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllDistrictsAsync_ShouldReturnFeaturesFromCache_WhenCacheIsPopulated()
    {
        var cachedFeatures = new List<Feature> { CreateFeature("CachedDistrict", "CD1", 30000, "Region2") };
        _memoryCache.Set(CacheKeys.AllDistricts, cachedFeatures, TimeSpan.FromHours(1));

        var result = await CollectAsync(_service.GetAllDistrictsAsync());

        result.Should().BeEquivalentTo(cachedFeatures);
        _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Never);
    }

    [Fact]
    public async Task FilterByNameAsync_ShouldReturnMatchingFeaturesFromCache()
    {
        var cachedFeatures = new List<Feature> { CreateFeature("DistrictA", "DA1", 40000, "Region3"), CreateFeature("DistrictB", "DB1", 60000, "Region4") };
        _memoryCache.Set(CacheKeys.AllDistricts, cachedFeatures, TimeSpan.FromHours(1));

        var result = await CollectAsync(_service.FilterByNameAsync(new List<string> { "A" }));

        result.Should().ContainSingle(f => f.Properties.Name == "DistrictA");
        _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Never);
    }

    [Fact]
    public async Task FilterByNameAsync_ShouldReturnMatchingFeaturesFromRepository_WhenCacheIsEmpty()
    {
        var features = new List<Feature> { CreateFeature("DistrictX", "DX1", 70000, "Region5"), CreateFeature("DistrictY", "DY1", 80000, "Region6") };
        _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

        var result = await CollectAsync(_service.FilterByNameAsync(new List<string> { "X" }));

        result.Should().ContainSingle(f => f.Properties.Name == "DistrictX");
        _repositoryMock.Verify(repo => repo.GetAllFeaturesAsync(), Times.Once);
    }

    [Fact]
    public async Task FilterByNameAsync_ShouldReturnEmpty_WhenNoMatchesFound()
    {
        var features = new List<Feature> { CreateFeature("DistrictA", "DA1", 50000, "Region7"), CreateFeature("DistrictB", "DB1", 60000, "Region8") };
        _repositoryMock.Setup(repo => repo.GetAllFeaturesAsync()).Returns(ToAsyncEnumerable(features));

        var result = await CollectAsync(_service.FilterByNameAsync(new List<string> { "Z" }));

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


    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> source, [EnumeratorCancellation] System.Threading.CancellationToken cancellationToken = default)
    {
        foreach (var item in source)
        {
            yield return item;
            await Task.Yield();
        }
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
