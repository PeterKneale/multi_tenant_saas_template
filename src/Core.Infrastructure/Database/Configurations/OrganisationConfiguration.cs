using Core.Domain.Organisations;
using Core.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Core.Infrastructure.Database.DbColumnNames;

namespace Core.Infrastructure.Database.Configurations;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<OrganisationIdConverter>()
            .HasColumnName(OrganisationIdColumn);

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(CreatedAt);

        builder
            .OwnsOne(p => p.Name, name =>
            {
                name.Property(p => p.Title).HasColumnName(Title);
                name.Property(p => p.Description).HasColumnName(Description);
            });

        builder
            .Ignore(x => x.DomainEvents);
    }
}