using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.Infrastructure.ORM.Mapping
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Define table name
            builder.ToTable("Carts");

            // Define primary key
            builder.HasKey(c => c.Id);

            // Define properties
            builder.Property(c => c.UserID)
                   .IsRequired();

            builder.Property(c => c.Date)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.TotalPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(c => c.TotalDescount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(c => c.TotalPriceWithDescount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Add any additional configurations if needed
        }
    }
}
