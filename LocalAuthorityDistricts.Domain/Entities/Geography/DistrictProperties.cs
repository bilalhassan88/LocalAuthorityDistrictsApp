namespace LocalAuthorityDistricts.Domain
{
    public class DistrictProperties
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public int Population { get; private set; }
        public string Region { get; private set; }

        public DistrictProperties(string name, string code, int population, string region)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Unknown District" : name;
            Code = string.IsNullOrWhiteSpace(code) ? "Unknown" : code;
            Population = population;
            Region = string.IsNullOrWhiteSpace(region) ? "Unknown" : region;
        }
    }
}
