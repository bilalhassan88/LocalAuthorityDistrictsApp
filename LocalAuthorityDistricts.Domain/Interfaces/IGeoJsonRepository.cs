namespace LocalAuthorityDistricts.Domain
{
    public interface IGeoJsonRepository
    {
        IAsyncEnumerable<Feature> GetAllFeaturesAsync();
    }
}
