﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPSSL.GameService.Domain.Models;

namespace RPSSL.GameService.Infrastructure.Persistence.Configurations;

public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
{
	public void Configure(EntityTypeBuilder<Choice> builder)
	{
		builder.Property(x => x.Id)
			.HasAnnotation("MinValue", 1)
			.HasAnnotation("MaxValue", 5);

		builder.Property(x => x.Name)
			.HasMaxLength(12)
			.IsRequired();

		builder.Property(x => x.Image)
			.HasMaxLength(int.MaxValue)
			.IsRequired();
	}
}