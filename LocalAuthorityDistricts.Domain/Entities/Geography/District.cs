namespace LocalAuthorityDistricts.Domain
{
    public class District : BaseEntity
    {
        public string Name { get; private set; }
        public DistrictType Type { get; private set; }
        public Feature GeoFeature { get; private set; }
        public BoundingBox BoundingBox { get; private set; }

        public District(
            string name,
            DistrictType type,
            Feature geoFeature,
            BoundingBox boundingBox)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type;
            GeoFeature = geoFeature ?? throw new ArgumentNullException(nameof(geoFeature));
            BoundingBox = boundingBox ?? throw new ArgumentNullException(nameof(boundingBox));
        }
    }
}
