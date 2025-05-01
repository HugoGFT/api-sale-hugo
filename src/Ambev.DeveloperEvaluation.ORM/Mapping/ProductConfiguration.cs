using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table name
            builder.ToTable("Products");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Description)
                   .HasMaxLength(1000);

            builder.Property(p => p.Category)
                   .HasMaxLength(100);

            builder.Property(p => p.Image)
                   .HasMaxLength(500);

            builder.Property(p => p.Rate)
                   .HasColumnType("decimal(3,2)");

            builder.Property(p => p.Count)
                   .IsRequired();

            // Indexes
            builder.HasIndex(p => p.Title).HasDatabaseName("IX_Product_Title");
        }
    }
}
