namespace LocalAuthorityDistricts.Domain
{
    // Represents the entire GeoJSON file as a FeatureCollection.
    public class GeoJson
    {
        public GeoJsonObjectType CollectionType { get; private set; } = GeoJsonObjectType.FeatureCollection;
        public List<Feature> Features { get; private set; }

        public GeoJson(List<Feature> features)
        {
            Features = features ?? new List<Feature>();
        }
    }
}
