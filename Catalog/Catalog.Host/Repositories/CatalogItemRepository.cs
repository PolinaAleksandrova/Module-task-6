using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
           .Include(i => i.CatalogBrand)
           .Include(i => i.Characteristic)
           .Skip(pageSize * pageIndex)
           .Take(pageSize)
           .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<CatalogItem> GetByIdAsync(int id)
    {
        var item = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.Characteristic)
            .Where(c => c.Id == id)
            .FirstAsync();

        return new CatalogItem() { Id = item.Id, CatalogBrandId = item.CatalogBrandId, CatalogBrand = item.CatalogBrand, Name = item.Name, CharacteristicId = item.CharacteristicId, Characteristic = item.Characteristic, Price = item.Price, PictureFileName = item.PictureFileName };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrandsAsync(int pageIndex, int pageSize)
    {
        var totalBrands = await _dbContext.CatalogBrands
            .LongCountAsync();

        var brandsOnPage = await _dbContext.CatalogBrands
            .OrderBy(c => c.Id)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogBrand>() { TotalCount = totalBrands, Data = brandsOnPage };
    }

    public async Task<int?> AddItemAsync(string name, int catalogBrandId, decimal price, int characteristicId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CharacteristicId = characteristicId,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<string?> RemoveItemAsync(int id)
    {
        var item = await _dbContext.CatalogItems.Where(c => c.Id == id).FirstAsync();
        _dbContext.CatalogItems.Remove(item);

        await _dbContext.SaveChangesAsync();

        return $"Deleted. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }

    public async Task<string?> UpdateItemAsync(int id, string name, int catalogBrandId, decimal price, int characteristicId, string pictureFileName)
    {
        var itemFromDb = await _dbContext.CatalogItems.Where(i => i.Id == id).FirstOrDefaultAsync();
        itemFromDb!.CatalogBrandId = catalogBrandId;
        itemFromDb.CharacteristicId = characteristicId;
        itemFromDb.Name = name;
        itemFromDb.PictureFileName = pictureFileName;
        itemFromDb.Price = price;
        _dbContext.CatalogItems.Update(itemFromDb);

        await _dbContext.SaveChangesAsync();

        return $"Updated. ({DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")})";
    }
}