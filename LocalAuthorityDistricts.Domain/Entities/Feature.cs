namespace LocalAuthorityDistricts.Domain
{
    public class Feature : BaseEntity
    {
        public GeoJsonObjectType ObjectType { get; private set; }
        public Geometry Geometry { get; private set; }
        public DistrictProperties Properties { get; private set; }

        public Feature(
            GeoJsonObjectType objectType,
            Geometry geometry,
            DistrictProperties properties)
        {
            ObjectType = objectType;

            Geometry = geometry ?? new Geometry(
                GeoJsonObjectType.Polygon,
                new List<List<List<double>>>());

            // Use the provided properties, or default to "Unknown"
            Properties = properties ?? new DistrictProperties(
                "Unknown",
                "Unknown",
                0,
                "Unknown"
            );
        }
    }
}
