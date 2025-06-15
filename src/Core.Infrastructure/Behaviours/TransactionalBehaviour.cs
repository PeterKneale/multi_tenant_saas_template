using Core.Infrastructure.Database;

namespace Core.Infrastructure.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse>(
    IUnitOfWork work,
    IDomainEventDispatcher dispatcher,
    ILogger<TRequest> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        log.LogDebug("📗 Transactional Behaviour");
        var name = typeof(TRequest).FullName.Split(".").Last();

        if (name.Contains("query", StringComparison.InvariantCultureIgnoreCase)) return await next();

        log.LogDebug($"{name} - Begin Unit of Work");
        var response = await next();
        await dispatcher.Publish(cancellationToken);
        log.LogDebug($"{name} - Completing Unit of Work");
        await work.SaveChangesAsync(cancellationToken);
        log.LogDebug($"{name} - Completed Unit of Work");

        return response;
    }
}