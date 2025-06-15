using Core.Domain.Users;
using Core.Infrastructure.Database.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Core.Infrastructure.Database.DbColumnNames;

namespace Core.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.Id)
            .HasConversion<UserIdConverter>()
            .HasColumnName(UserIdColumn);

        builder
            .Property(e => e.OrganisationId)
            .HasConversion<OrganisationIdConverter>()
            .HasColumnName(OrganisationIdColumn);

        builder
            .OwnsOne(p => p.Name, name =>
            {
                name.Property(p => p.FirstName).HasColumnName(FirstName);
                name.Property(p => p.LastName).HasColumnName(LastName);
            });

        builder.Property(p => p.Email)
            .HasColumnName(Email)
            .HasConversion<EmailAddressConverter>();

        builder.Property(p => p.Password).HasColumnName(Password);

        builder
            .Property(e => e.Role)
            .HasConversion(x => x.Value, x => UserRole.Create(x))
            .HasColumnName(Role);

        builder
            .Property(e => e.Verified)
            .HasColumnName(Verified);

        builder
            .Property(e => e.VerifiedToken)
            .HasColumnName(VerifiedToken);

        builder
            .Property(e => e.ForgottenToken)
            .HasColumnName(ForgotToken);

        builder
            .Property(e => e.ForgottenTokenExpiry)
            .HasColumnName(ForgotTokenExpiry);

        builder
            .Property(e => e.CreatedAt)
            .HasColumnName(CreatedAt);

        builder
            .Ignore(x => x.DomainEvents);
    }
}