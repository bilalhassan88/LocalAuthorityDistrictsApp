    namespace LocalAuthorityDistricts.Domain
    {
        public interface ICoordinates { }

        public class PointCoordinates : ICoordinates
        {
            public List<double> Coordinates { get; private set; }

            public PointCoordinates(List<double> coordinates)
            {
                Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "Point coordinates cannot be null");
            }
        }

        public class LineStringCoordinates : ICoordinates
        {
            public List<List<double>> Coordinates { get; private set; }

            public LineStringCoordinates(List<List<double>> coordinates)
            {
                Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "LineString coordinates cannot be null");
            }
        }

        public class PolygonCoordinates : ICoordinates
        {
            public List<List<List<double>>> Coordinates { get; private set; }

            public PolygonCoordinates(List<List<List<double>>> coordinates)
            {
                Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "Polygon coordinates cannot be null");
            }
        }

        public class MultiPolygonCoordinates : ICoordinates
        {
            public List<List<List<List<double>>>> Coordinates { get; private set; }

            public MultiPolygonCoordinates(List<List<List<List<double>>>> coordinates)
            {
                Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiPolygon coordinates cannot be null");
            }
        }
    public class MultiPointCoordinates : ICoordinates
    {
        public List<List<double>> Coordinates { get; private set; }

        public MultiPointCoordinates(List<List<double>> coordinates)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiPoint coordinates cannot be null");
        }
    }

    public class MultiLineStringCoordinates : ICoordinates
    {
        public List<List<List<double>>> Coordinates { get; private set; }

        public MultiLineStringCoordinates(List<List<List<double>>> coordinates)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiLineString coordinates cannot be null");
        }
    }
}