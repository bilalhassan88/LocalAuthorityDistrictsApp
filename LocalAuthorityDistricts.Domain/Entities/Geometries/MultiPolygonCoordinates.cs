using LocalAuthorityDistricts.Domain;

public class MultiPolygonCoordinates : ICoordinates
{
    public List<List<List<List<double>>>> Coordinates { get; private set; }

    public MultiPolygonCoordinates(List<List<List<List<double>>>> coordinates)
    {
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "MultiPolygon coordinates cannot be null");
    }
}