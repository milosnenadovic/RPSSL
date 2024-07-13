using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
	public void Configure(EntityTypeBuilder<Language> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(40)
			.IsRequired();

		builder.Property(x => x.LanguageCode)
			.HasMaxLength(10)
			.IsRequired();

		builder.Property(x => x.CountryId)
			.HasMaxLength(2)
			.IsRequired();

		builder.HasKey(x => x.Id);
	}
}