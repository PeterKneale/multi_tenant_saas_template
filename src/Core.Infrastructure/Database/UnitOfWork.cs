namespace Core.Infrastructure.Database;

public class UnitOfWork(DatabaseContext db, ILogger<UnitOfWork> logs) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        logs.LogInformation("💾 Saving changes");
        await db.SaveChangesAsync(cancellationToken);
    }
}