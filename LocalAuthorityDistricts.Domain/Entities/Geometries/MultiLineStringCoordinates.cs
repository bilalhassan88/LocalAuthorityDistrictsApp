namespace LocalAuthorityDistricts.Domain
{
    public class MultiLineStringCoordinates : ICoordinates
    {
        public List<List<List<double>>> Coordinates { get; private set; }

        public MultiLineStringCoordinates(List<List<List<double>>> coordinates)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiLineString coordinates cannot be null");
        }
    }
}
