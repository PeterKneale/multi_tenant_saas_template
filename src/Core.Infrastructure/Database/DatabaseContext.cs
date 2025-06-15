using Core.Application.Contracts;
using Core.Domain.Invitations;
using Core.Domain.Organisations;
using Core.Domain.Users;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Core.Infrastructure.Database;

public class DatabaseContext(ICurrentContext context, DbContextOptions<DatabaseContext> options) :
    DbContext(options), IDataProtectionKeyContext
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<Organisation> Organisations { get; init; } = null!;
    public DbSet<Invitation> Invitations { get; init; } = null!;
    public DbSet<DataProtectionKey> DataProtectionKeys { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.Entity<Organisation>().HasQueryFilter(x => x.Id.Equals(context.OrganisationId));
        modelBuilder.Entity<User>().HasQueryFilter(x => x.OrganisationId.Equals(context.OrganisationId));
        modelBuilder.Entity<Invitation>().HasQueryFilter(x => x.OrganisationId.Equals(context.OrganisationId));
    }
}