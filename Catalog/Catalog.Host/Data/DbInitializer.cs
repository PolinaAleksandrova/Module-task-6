using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
namespace Catalog.Host.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.CatalogBrands.Any())
        {
            await context.CatalogBrands.AddRangeAsync(GetPreconfiguredCatalogBrands());

            await context.SaveChangesAsync();
        }

        if (!context.Characteristics.Any())
        {
            await context.Characteristics.AddRangeAsync(GetPreconfiguredCharacteristics());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>()
        {
            new CatalogBrand() { Author = "Stephen King" },
            new CatalogBrand() { Author = "Oscar Wilde" }
        };
    }

    private static IEnumerable<Characteristic> GetPreconfiguredCharacteristics()
    {
        return new List<Characteristic>()
        {
            new Characteristic() { Publisher = "Independently Published", YearOfPublishing = 2020, NumberOfPages = 272, Language = "English" },
            new Characteristic() { Publisher = "Fall River Press", YearOfPublishing = 2014, NumberOfPages = 300, Language = "English" },
            new Characteristic() { Publisher = "Dover Publications, Incorporated", YearOfPublishing = 1990, NumberOfPages = 64, Language = "English" },
            new Characteristic() { Publisher = "HarperCollins Publishers Limited", YearOfPublishing = 2000, NumberOfPages = 1280, Language = "English" },
            new Characteristic() { Publisher = "Penguin Publishing Group", YearOfPublishing = 1986, NumberOfPages = 352, Language = "English" },
            new Characteristic() { Publisher = "Anchor", YearOfPublishing = 2011, NumberOfPages = 304, Language = "English" },
            new Characteristic() { Publisher = "Doubleday", YearOfPublishing = 1990, NumberOfPages = 464, Language = "English" },
            new Characteristic() { Publisher = "Anchor", YearOfPublishing = 2013, NumberOfPages = 688, Language = "English" },
            new Characteristic() { Publisher = "New English Library", YearOfPublishing = 1983, NumberOfPages = 224, Language = "English" },
            new Characteristic() { Publisher = "Doubleday", YearOfPublishing = 1990, NumberOfPages = 1152, Language = "English" },
            new Characteristic() { Publisher = "Scribner", YearOfPublishing = 2018, NumberOfPages = 176, Language = "English" },
            new Characteristic() { Publisher = "Gallery Books", YearOfPublishing = 2016, NumberOfPages = 320, Language = "English" }
        };
    }

    private static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>()
        {
            new CatalogItem { CatalogBrandId = 1, CharacteristicId = 1, Name = "The Picture of Dorian Gray", Price = 46, PictureFileName = "1.jpg" },
            new CatalogItem { CatalogBrandId = 1, CharacteristicId = 2, Name = "The Picture of Dorian Gray & Other Works", Price = 7, PictureFileName = "2.jpg" },
            new CatalogItem { CatalogBrandId = 1, CharacteristicId = 3, Name = "The Importance of Being Earnest", Price = 4, PictureFileName = "3.jpg" },
            new CatalogItem { CatalogBrandId = 1, CharacteristicId = 4, Name = "Collins Complete Works of Oscar Wilde", Price = 15, PictureFileName = "4.jpg" },
            new CatalogItem { CatalogBrandId = 1, CharacteristicId = 5, Name = "The Importance of Being Earnest and Other Plays", Price = 5, PictureFileName = "5.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 6, Name = "Carrie", Price = 8, PictureFileName = "6.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 7, Name = "Salem's Lot", Price = 21, PictureFileName = "7.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 8, Name = "The Shining", Price = 10, PictureFileName = "8.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 9, Name = "Rage", Price = 15, PictureFileName = "9.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 10, Name = "The Stand: The Complete and Uncut Edition", Price = 7, PictureFileName = "10.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 11, Name = "The Mist", Price = 7, PictureFileName = "11.jpg" },
            new CatalogItem { CatalogBrandId = 2, CharacteristicId = 12, Name = "The Long Walk", Price = 9, PictureFileName = "12.jpg" }
        };
    }
}