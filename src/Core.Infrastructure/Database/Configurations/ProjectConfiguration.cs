using Core.Domain.Projects;
using Core.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Core.Infrastructure.Database.DbColumnNames;

namespace Core.Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.ProjectId);
        
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