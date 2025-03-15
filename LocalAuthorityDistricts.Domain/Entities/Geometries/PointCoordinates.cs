using LocalAuthorityDistricts.Domain;

public class PointCoordinates : ICoordinates
{
    public List<double> Coordinates { get; private set; }

    public PointCoordinates(List<double> coordinates)
    {
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "Point coordinates cannot be null");
    }
}