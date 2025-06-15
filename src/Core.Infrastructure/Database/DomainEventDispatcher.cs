namespace Core.Infrastructure.Database;

internal class DomainEventDispatcher(DatabaseContext db, IPublisher mediator, ILogger<DomainEventDispatcher> log)
    : IDomainEventDispatcher
{
    public async Task Publish(CancellationToken token = default)
    {
        var entities = db.ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in entities)
        {
            log.LogDebug($"Publishing domain events from {entity.GetType().Name}");
            foreach (var domainEvent in entity.DomainEvents)
            {
                log.LogDebug($"Publishing domain event {domainEvent.GetType().Name}");
                await mediator.Publish(domainEvent, token);
            }

            entity.ClearDomainEvents();
        }
    }
}