using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id);

        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(u => u.UpdatedAt).HasDefaultValueSql("now()");
        builder.Property(u => u.Street).HasMaxLength(100);
        builder.Property(u => u.Number).HasMaxLength(10);
        builder.Property(u => u.City).HasMaxLength(50);
        builder.Property(u => u.ZipCode).HasMaxLength(20);
        builder.Property(u => u.Lat).HasMaxLength(20);
        builder.Property(u => u.Long).HasMaxLength(20);
        builder.Property(u => u.Firstname).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Lastname).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

    }
}
