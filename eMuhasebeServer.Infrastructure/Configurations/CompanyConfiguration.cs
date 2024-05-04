﻿using eMuhasebeServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eMuhasebeServer.Infrastructure.Configurations;
internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(x => x.TaxNumber).HasColumnType("varchar(11)");

        builder.OwnsOne(p => p.Database, builder =>
        {
            builder.Property(property => property.Server).HasColumnName("Server");
            builder.Property(property => property.DatabaseName).HasColumnName("DatabaseName");
            builder.Property(property => property.UserId).HasColumnName("UserId");
            builder.Property(property => property.Password).HasColumnName("Password");
        });

    }
}