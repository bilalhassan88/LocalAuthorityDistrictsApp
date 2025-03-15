using LocalAuthorityDistricts.Domain;

public class PolygonCoordinates : ICoordinates
{
    public List<List<List<double>>> Coordinates { get; private set; }

    public PolygonCoordinates(List<List<List<double>>> coordinates)
    {
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "Polygon coordinates cannot be null");
    }
}