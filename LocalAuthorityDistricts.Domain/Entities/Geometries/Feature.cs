using System.Text.Json.Serialization;

namespace LocalAuthorityDistricts.Domain
{
    public class Feature : BaseEntity
    {
        [JsonPropertyName("type")]
        public GeoJsonObjectType ObjectType { get; private set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; private set; }

        [JsonPropertyName("properties")]
        public DistrictProperties Properties { get; private set; }

        [JsonConstructor]
        public Feature(
            GeoJsonObjectType objectType,
            Geometry geometry,
            DistrictProperties properties
        )
        {
            ObjectType = objectType;
            Geometry = geometry;
            Properties = properties;
        }
    }
}