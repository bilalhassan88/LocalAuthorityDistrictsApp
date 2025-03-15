using Bunit;
using LocalAuthorityDistricts.Application;
using LocalAuthorityDistricts.Domain;
using LocalAuthorityDistricts.Presentation.BlazorServer.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.JSInterop;
using LocalAuthorityDistricts.Infrastructure;
using Index = LocalAuthorityDistricts.Presentation.BlazorServer.Pages.Index;

namespace BlazorTests
{
    public class BlazorFrontendTests : Bunit.TestContext
    {
        private readonly Mock<IGeoJsonService> _geoJsonServiceMock;
        private readonly Mock<ICityService> _cityServiceMock;
        private readonly Mock<IJSRuntime> _jsRuntimeMock;
        private readonly IOptions<ChunkSettings> _chunkSettings;
        private readonly IOptions<MapboxSettings> _mapboxSettings;
        private readonly IOptions<GeoJsonFileSettings> _geoJsonFileSettings;

        public BlazorFrontendTests()
        {
            _geoJsonServiceMock = new Mock<IGeoJsonService>();
            _cityServiceMock = new Mock<ICityService>();
            _jsRuntimeMock = new Mock<IJSRuntime>();

            _chunkSettings = Options.Create(new ChunkSettings { ChunkSize = 20 });
            _mapboxSettings = Options.Create(new MapboxSettings { AccessToken = "pk.eyJ1IjoiYmlsYWxoYXNzYW4iLCJhIjoiY2pzZXBtZDlrMTVjZzQ0bzZmY29zNHozbCJ9.gIAwBp9pMmZUhWliI4CrfA" });
            _geoJsonFileSettings = Options.Create(new GeoJsonFileSettings { FilePath = "../LocalAuthorityDistricts.Infrastructure/Data/local-authority-district.geojson" });

            Services.AddSingleton(_geoJsonServiceMock.Object);
            Services.AddScoped(_ => _cityServiceMock.Object);
            Services.AddSingleton(_jsRuntimeMock.Object);
            Services.AddSingleton(_chunkSettings);
            Services.AddSingleton(_mapboxSettings);
            Services.AddSingleton(_geoJsonFileSettings);
        }

        [Fact]
        public void Component_Should_Render_Correctly()
        {
            var cut = RenderComponent<Index>();

            cut.Markup.Should().Contain("Local Authority Districts");
        }
    }
}
