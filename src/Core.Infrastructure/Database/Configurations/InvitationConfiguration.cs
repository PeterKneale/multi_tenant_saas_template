using Core.Domain.Invitations;
using Core.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Core.Infrastructure.Database.DbColumnNames;

namespace Core.Infrastructure.Database.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<InvitationIdConverter>()
            .HasColumnName(InvitationIdColumn);

        builder
            .Property(e => e.OrganisationId)
            .HasConversion<OrganisationIdConverter>()
            .HasColumnName(OrganisationIdColumn);

        builder
            .Property(e => e.UserId)
            .HasConversion<UserIdConverter>()
            .HasColumnName(UserIdColumn);

        builder
            .Property(p => p.Email)
            .HasColumnName(Email)
            .HasConversion<EmailAddressConverter>();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(CreatedAt);

        builder
            .Ignore(x => x.DomainEvents);
    }
}