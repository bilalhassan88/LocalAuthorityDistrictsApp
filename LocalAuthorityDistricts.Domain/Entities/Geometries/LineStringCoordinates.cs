using LocalAuthorityDistricts.Domain;

public class LineStringCoordinates : ICoordinates
{
    public List<List<double>> Coordinates { get; private set; }

    public LineStringCoordinates(List<List<double>> coordinates)
    {
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates), "LineString coordinates cannot be null");
    }
}