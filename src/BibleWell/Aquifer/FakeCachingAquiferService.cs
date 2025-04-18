namespace BibleWell.Aquifer;

/// <summary>
/// A fake implementation of <see cref="ICachingAquiferService"/> for design-time use.
/// </summary>
public sealed class FakeCachingAquiferService : ICachingAquiferService
{
    public Task<ResourceContent?> GetResourceContentAsync(int contentId)
    {
        return Task.FromResult<ResourceContent?>(null);
    }
}