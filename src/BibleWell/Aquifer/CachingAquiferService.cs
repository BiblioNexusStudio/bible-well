namespace BibleWell.Aquifer;

public class CachingAquiferService(IReadOnlyAquiferService _readOnlyAquiferService, IReadWriteAquiferService _readWriteAquiferService)
    : ICachingAquiferService
{
    public async Task<ResourceContent?> GetResourceContentAsync(int contentId)
    {
        var resource = await _readWriteAquiferService.GetResourceContentAsync(contentId);
        if (resource == null)
        {
            resource = await _readOnlyAquiferService.GetResourceContentAsync(contentId);
            if (resource != null)
            {
                await _readWriteAquiferService.SaveResourceContentAsync(resource);
            }
        }

        return resource;
    }
}