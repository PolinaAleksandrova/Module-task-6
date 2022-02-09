using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter);
    Task<CatalogItem> GetByIdAsync(int id);
    Task<PaginatedItems<CatalogBrand>> GetBrandsAsync(int pageIndex, int pageSize);
    Task<int?> AddItemAsync(string name, int catalogBrandId, decimal price, int characteristicId, string pictureFileName);
    Task<string?> RemoveItemAsync(int id);
    Task<string?> UpdateItemAsync(int id, string name, int catalogBrandId, decimal price, int characteristicId, string pictureFileName);
}