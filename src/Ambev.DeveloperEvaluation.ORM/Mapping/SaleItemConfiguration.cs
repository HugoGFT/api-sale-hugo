using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            // Define table name
            builder.ToTable("SaleItems");

            // Define primary key
            builder.HasKey(si => si.Id);

            // Define properties
            builder.Property(si => si.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(si => si.SaleId)
                   .IsRequired();

            builder.Property(si => si.ProductId)
                   .IsRequired();

            builder.Property(si => si.Quantity)
                   .IsRequired();

            builder.Property(si => si.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(si => si.TotalAmount)
                   .HasColumnType("decimal(18,2)");
            builder.Property(si => si.Total)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
