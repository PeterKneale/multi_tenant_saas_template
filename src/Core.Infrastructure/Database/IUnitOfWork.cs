namespace Core.Infrastructure.Database;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}