namespace LocalAuthorityDistricts.Domain
{
    // Fetches raw Feature data from your static .geojson file
    public interface IGeoJsonRepository
    {
        Task<List<Feature>> GetAllFeaturesAsync();
    }
}
