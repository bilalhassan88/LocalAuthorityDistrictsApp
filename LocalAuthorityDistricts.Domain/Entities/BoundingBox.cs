namespace LocalAuthorityDistricts.Domain
{
    public class BoundingBox
    {
        public double MinLongitude { get; private set; }
        public double MinLatitude { get; private set; }
        public double MaxLongitude { get; private set; }
        public double MaxLatitude { get; private set; }

        public BoundingBox(double minLon, double minLat, double maxLon, double maxLat)
        {
            MinLongitude = minLon;
            MinLatitude = minLat;
            MaxLongitude = maxLon;
            MaxLatitude = maxLat;
        }
    }
}
