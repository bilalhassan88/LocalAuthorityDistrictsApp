namespace LocalAuthorityDistricts.Domain
{
    public interface IGeoJsonRepository
    {
        Task<List<Feature>> GetAllFeaturesAsync();
    }
}
