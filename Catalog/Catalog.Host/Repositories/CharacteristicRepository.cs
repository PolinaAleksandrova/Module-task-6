using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CharacteristicRepository : ICharacteristicRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CharacteristicRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<int?> AddCharacteristicAsync(string publisher, int yearOfPublishing, int numberOfPages, string language)
    {
        var newCharact = await _dbContext.Characteristics.AddAsync(new Characteristic
        {
            Publisher = publisher,
            YearOfPublishing = yearOfPublishing,
            NumberOfPages = numberOfPages,
            Language = language
        });

        await _dbContext.SaveChangesAsync();

        return newCharact.Entity.Id;
    }

    public async Task<string?> RemoveCharacteristicAsync(int id)
    {
        var charact = await _dbContext.Characteristics.Where(c => c.Id == id).FirstOrDefaultAsync();
        _dbContext.Characteristics.Remove(charact!);

        await _dbContext.SaveChangesAsync();

        return $"Deleted. ({DateTime.UtcNow:dddd, dd MMMM yyyy HH:mm:ss})";
    }

    public async Task<string?> UpdateCharacteristicAsync(int id, string publisher, int yearOfPublishing, int numberOfPages, string language)
    {
        var charactFromDb = await _dbContext.Characteristics.Where(i => i.Id == id).FirstOrDefaultAsync();
        charactFromDb!.Publisher = publisher;
        charactFromDb.YearOfPublishing = yearOfPublishing;
        charactFromDb.NumberOfPages = numberOfPages;
        charactFromDb.Language = language;

        _dbContext.Characteristics.Update(charactFromDb);

        await _dbContext.SaveChangesAsync();

        return $"Updated. ({DateTime.UtcNow:dddd, dd MMMM yyyy HH:mm:ss})";
    }
}