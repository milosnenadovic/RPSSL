using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.UserName)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasIndex(x => x.UserName);
    }
}