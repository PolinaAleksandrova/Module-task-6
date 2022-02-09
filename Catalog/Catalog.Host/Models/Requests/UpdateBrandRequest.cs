using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class UpdateBrandRequest
{
    public int Id { get; set; }
    public string Author { get; set; } = null!;
}