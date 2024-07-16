using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class ChoiceWinConfiguration : IEntityTypeConfiguration<ChoiceWin>
{
    public void Configure(EntityTypeBuilder<ChoiceWin> builder)
    {
        builder.Property(x => x.ChoiceId)
            .HasAnnotation("MinValue", 1)
            .HasAnnotation("MaxValue", 5)
            .IsRequired();

        builder.Property(x => x.BeatsChoiceId)
            .HasAnnotation("MinValue", 1)
            .HasAnnotation("MaxValue", 5)
            .IsRequired();

        builder.Property(x => x.ActionName)
            .HasMaxLength(20)
            .IsRequired();
    }
}