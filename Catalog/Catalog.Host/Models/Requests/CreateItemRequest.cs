using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class CreateItemRequest
{
    public int CatalogBrandId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int CharacteristicId { get; set; }
    public string PictureFileName { get; set; } = null!;
}