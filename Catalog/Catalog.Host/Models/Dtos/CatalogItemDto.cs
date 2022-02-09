using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Dtos;

public class CatalogItemDto
{
    public int Id { get; set; }
    public int CatalogBrandId { get; set; }
    public CatalogBrand CatalogBrand { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int CharacteristicId { get; set; }
    public Characteristic Characteristic { get; set; } = null!;
    public string PictureFileName { get; set; } = null!;
}