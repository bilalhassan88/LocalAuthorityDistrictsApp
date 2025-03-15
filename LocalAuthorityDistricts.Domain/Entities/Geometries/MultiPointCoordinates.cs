namespace LocalAuthorityDistricts.Domain
{
    public class MultiPointCoordinates : ICoordinates
    {
        public List<List<double>> Coordinates { get; private set; }

        public MultiPointCoordinates(List<List<double>> coordinates)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiPoint coordinates cannot be null");
        }
    }
}
