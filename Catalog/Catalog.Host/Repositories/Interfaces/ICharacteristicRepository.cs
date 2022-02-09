namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICharacteristicRepository
    {
        Task<int?> AddCharacteristicAsync(string publisher, int yearOfPublishing, int numberOfPages, string language);
        Task<string?> RemoveCharacteristicAsync(int id);
        Task<string?> UpdateCharacteristicAsync(int id, string publisher, int yearOfPublishing, int numberOfPages, string language);
    }
}
