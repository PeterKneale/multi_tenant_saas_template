using FluentValidation;

namespace Core.Infrastructure.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<TRequest> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        log.LogDebug("📗 Validation Behaviour");
        var name = typeof(TRequest).FullName.Split(".").Last();

        if (!validators.Any())
        {
            log.LogWarning("{Name} has no validators", name);
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count == 0)
        {
            log.LogDebug("{Name} is valid", name);
            return await next();
        }

        var errors = string.Join(",", failures.Select(x => x.ErrorMessage));
        log.LogError("{Name} Failed validation {@Request} {errors}", name, request, errors);
        throw new RequestValidationException(errors);
    }
}