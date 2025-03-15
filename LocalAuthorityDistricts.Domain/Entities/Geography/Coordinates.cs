namespace LocalAuthorityDistricts.Domain.Entities
{
    public class Coordinates
    {
        public List<List<double>> Points { get; private set; }

        public Coordinates(List<List<double>> points)
        {
            Points = points ?? new List<List<double>>();
        }
    }
}