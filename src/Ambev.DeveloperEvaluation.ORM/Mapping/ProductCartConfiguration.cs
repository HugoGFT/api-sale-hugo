using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class ProductCartConfiguration : IEntityTypeConfiguration<ProductCart>
    {
        public void Configure(EntityTypeBuilder<ProductCart> builder)
        {
            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.IdUser)
                .IsRequired();

            builder.Property(pc => pc.IdCart)
                .IsRequired();

            builder.Property(pc => pc.IdProduct)
                .IsRequired();

            builder.Property(pc => pc.Count)
                .IsRequired();
        }
    }
}
