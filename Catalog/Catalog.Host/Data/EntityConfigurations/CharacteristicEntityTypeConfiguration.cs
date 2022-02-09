using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public class CharacteristicEntityTypeConfiguration
    : IEntityTypeConfiguration<Characteristic>
{
    public void Configure(EntityTypeBuilder<Characteristic> builder)
    {
        builder.ToTable("Characteristic");

        builder.Property(ci => ci.Id)
            .UseHiLo("characteristic_hilo")
            .IsRequired();

        builder.Property(ci => ci.Publisher)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(ci => ci.YearOfPublishing)
            .IsRequired(true);

        builder.Property(ci => ci.NumberOfPages)
            .IsRequired(true);

        builder.Property(ci => ci.Language)
            .IsRequired(true)
            .HasMaxLength(50);
    }
}