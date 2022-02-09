namespace Catalog.Host.Data.Entities
{
    public class Characteristic
    {
        public int Id { get; set; }
        public string Publisher { get; set; } = null!;
        public int YearOfPublishing { get; set; }
        public int NumberOfPages { get; set; }
        public string Language { get; set; } = null!;
    }
}
