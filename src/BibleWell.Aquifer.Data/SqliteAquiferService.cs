namespace BibleWell.Aquifer.Data;

public sealed class SqliteAquiferService : IReadWriteAquiferService
{
    private readonly ResourceContentRepository _resourceContentRepository;

    public SqliteAquiferService()
    {
        var dbManager = new SqliteDbManager(Constants.ConnectionString);
        _resourceContentRepository = new ResourceContentRepository(dbManager);
    }

    public Task<ResourceContent?> GetResourceContentAsync(int id)
    {
        return _resourceContentRepository.GetByIdAsync(id);
    }

    public Task SaveResourceContentAsync(ResourceContent resourceContent)
    {
        return _resourceContentRepository.SaveAsync(resourceContent);
    }

    // add other repositories here ...
}