using eMuhasebeServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eMuhasebeServer.Infrastructure.Configurations;
internal sealed class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder.HasKey(c => new { c.AppUserId, c.CompanyId });
        builder.HasQueryFilter(filter => !filter.Company!.IsDeleted);
    }
}
