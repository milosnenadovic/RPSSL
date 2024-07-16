using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class LocalizationLabelConfiguration : IEntityTypeConfiguration<LocalizationLabel>
{
    public void Configure(EntityTypeBuilder<LocalizationLabel> builder)
    {
        builder.Property(x => x.Key)
            .HasMaxLength(48)
            .IsRequired();

        builder.Property(x => x.LanguageId)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(192)
            .IsRequired();

        builder.HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId);

        builder.HasKey(x => new { x.Key, x.LanguageId });
    }
}