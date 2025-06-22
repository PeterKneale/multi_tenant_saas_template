using Core.Domain.Features;
using Core.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Core.Infrastructure.Database.DbColumnNames;

namespace Core.Infrastructure.Database.Configurations;

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.HasKey(x => x.FeatureId);
        
        builder
            .Property(e => e.FeatureId)
            .HasConversion<FeatureIdConverter>()
            .HasColumnName(FeatureIdColumn);
        
        builder
            .Property(e => e.ProjectId)
            .HasConversion<ProjectIdConverter>()
            .HasColumnName(ProjectIdColumn);

        builder
            .Property(e => e.OrganisationId)
            .HasConversion<OrganisationIdConverter>()
            .HasColumnName(OrganisationIdColumn);
        
        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(CreatedAt);

        builder
            .Property(e => e.CreatedBy)
            .HasConversion<UserIdConverter>()
            .HasColumnName(CreatedBy);
        
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