﻿using Hydra.Cms.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;


namespace Hydra.Cms.Core.EntityConfiguration
{
    public class LinkSectionConfiguration : IEntityTypeConfiguration<LinkSection>
    {
        public void Configure(EntityTypeBuilder<LinkSection> builder)
        {
            builder.ToTable("LinkSection", "Cms");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title).HasMaxLength(300);

        }
    }
}
