using Newtonsoft.Json;

namespace Core.Infrastructure.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        log.LogDebug("📗 Logging Behaviour");
        var name = typeof(TRequest).FullName.Split(".").Last();
        var type = typeof(TRequest);
        var sensitive = type.GetCustomAttributes(typeof(SensitiveDataAttribute), false).Length != 0;
        var shouldLog = type.GetCustomAttributes(typeof(NoLogAttribute), false).Length == 0;
        var body = sensitive ? "*** Sensitive Data ***" : JsonConvert.SerializeObject(request);

        if (shouldLog) log.LogInformation("🚀 Executing: {Name} - {Body}", name, body);

        TResponse result;
        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            log.LogError(e, "💥Error Executing: {Name} - {Body}", name, body);
            throw;
        }

        return result;
    }
}