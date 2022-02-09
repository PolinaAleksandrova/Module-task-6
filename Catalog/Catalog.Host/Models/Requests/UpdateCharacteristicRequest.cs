using Catalog.Host.Data.Entities;

namespace Catalog.Host.Models.Requests;

public class UpdateCharacteristicRequest
{
    public int Id { get; set; }
    public string Publisher { get; set; } = null!;
    public int YearOfPublishing { get; set; }
    public int NumberOfPages { get; set; }
    public string Language { get; set; } = null!;
}