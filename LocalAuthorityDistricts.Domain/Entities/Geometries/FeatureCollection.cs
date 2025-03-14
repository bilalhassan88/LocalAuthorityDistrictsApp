using LocalAuthorityDistricts.Domain;
using System.Text.Json.Serialization;

public class FeatureCollection
{
    [JsonPropertyName("type")]
    public GeoJsonObjectType Type { get; set; }

    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; }
}