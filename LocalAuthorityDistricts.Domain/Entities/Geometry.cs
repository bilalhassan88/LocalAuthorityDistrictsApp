using System.Text.Json.Serialization;

namespace LocalAuthorityDistricts.Domain
{
    [JsonConverter(typeof(GeometryJsonConverter))]
    public class Geometry
    {
        public GeoJsonObjectType GeometryType { get; }
        public ICoordinates Coordinates { get; }

        public Geometry(GeoJsonObjectType geometryType, ICoordinates coordinates)
        {
            GeometryType = geometryType;
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        }
    }
}