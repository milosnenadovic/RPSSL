using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class ChoicesHistoryConfiguration : IEntityTypeConfiguration<ChoicesHistory>
{
	public void Configure(EntityTypeBuilder<ChoicesHistory> builder)
	{
		builder.Property(x => x.PlayerChoiceId)
			.HasAnnotation("MinValue", 1)
			.HasAnnotation("MaxValue", 5)
			.IsRequired();

		builder.Property(x => x.ComputerChoiceId)
			.HasAnnotation("MinValue", 1)
			.HasAnnotation("MaxValue", 5)
			.IsRequired();

		builder.Property(x => x.PlayerId)
			.HasMaxLength(36)
			.IsRequired();

		builder.HasIndex(x => x.PlayerId);
	}
}