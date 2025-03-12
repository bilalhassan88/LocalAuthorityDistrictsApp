namespace LocalAuthorityDistricts.Domain
{
    // The geometry portion of a Feature (Polygon, MultiPolygon, etc.).
    public class Geometry
    {
        public GeoJsonObjectType GeometryType { get; private set; }
        public List<List<List<double>>> Coordinates { get; private set; }

        public Geometry(GeoJsonObjectType geometryType, List<List<List<double>>> coordinates)
        {
            GeometryType = geometryType;
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        }
    }
}
