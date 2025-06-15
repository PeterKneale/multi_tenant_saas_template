namespace Core.Infrastructure.Database;

public interface IDomainEventDispatcher
{
    Task Publish(CancellationToken token = default);
}