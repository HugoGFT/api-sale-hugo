using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Domain.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Date)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(s => s.CustomerId)
                .IsRequired();

            builder.Property(s => s.Branch)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.TotalDiscount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.TotalWithDiscount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.IsCancelled)
                .IsRequired();
        }
    }
}