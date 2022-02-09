using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<int?> AddBrandAsync(string author)
    {
        var newBrand = await _dbContext.CatalogBrands.AddAsync(new CatalogBrand
        {
            Author = author
        });

        await _dbContext.SaveChangesAsync();

        return newBrand.Entity.Id;
    }

    public async Task<string?> RemoveBrandAsync(int id)
    {
        var brand = await _dbContext.CatalogBrands.Where(c => c.Id == id).FirstOrDefaultAsync();
        _dbContext.CatalogBrands.Remove(brand!);

        await _dbContext.SaveChangesAsync();

        return $"Deleted. ({DateTime.UtcNow:dddd, dd MMMM yyyy HH:mm:ss})";
    }

    public async Task<string?> UpdateBrandAsync(int id, string author)
    {
        var brandFromDb = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstOrDefaultAsync();
        brandFromDb!.Author = author;
        _dbContext.CatalogBrands.Update(brandFromDb);

        await _dbContext.SaveChangesAsync();

        return $"Updated. ({DateTime.UtcNow:dddd, dd MMMM yyyy HH:mm:ss})";
    }
}