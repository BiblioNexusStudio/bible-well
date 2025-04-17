namespace BibleWell.Aquifer;

public interface IReadWriteAquiferService : IReadOnlyAquiferService
{
    Task SaveResourceContentAsync(ResourceContent resource);
}