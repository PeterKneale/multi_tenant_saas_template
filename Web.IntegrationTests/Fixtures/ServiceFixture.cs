using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;

namespace Web.IntegrationTests.Fixtures;

public class ServiceFixture : WebApplicationFactory<Code.Extensions.Web>, ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(x => x.AddXUnit(this));
        builder.ConfigureServices(x => { x.AddScoped<ICurrentContext, FakeCurrentContext>(); });
    }

    public AsyncServiceScope CreateAsyncScope()
    {
        return Services.CreateAsyncScope();
    }

    public void ClearCacheKey(string key)
    {
        OutputHelper?.WriteLine($"Clearing cache key: {key}");
        var cache = Services.GetRequiredService<IMemoryCache>();
        cache.Remove(key);
    }

    public async Task Command(IRequest request)
    {
        await using var scope = CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(request);
    }

    public async Task Command(IRequest request, Guid organisationId, Guid userId)
    {
        await using var scope = CreateAsyncScope();
        SetTestContext(scope, organisationId, userId);
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(request);
    }

    public async Task<TResponse> Query<TResponse>(IRequest<TResponse> request)
    {
        await using var scope = CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(request);
    }

    public async Task<TResponse> Query<TResponse>(IRequest<TResponse> request, Guid organisationId, Guid userId)
    {
        await using var scope = CreateAsyncScope();
        SetTestContext(scope, organisationId, userId);
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(request);
    }

    private static void SetTestContext(IServiceScope scope, Guid organisationId, Guid userId)
    {
        var context = scope.ServiceProvider.GetRequiredService<ICurrentContext>() as FakeCurrentContext
                      ?? throw new Exception("No context available");
        context.OrganisationId = OrganisationId.Create(organisationId);
        context.UserId = UserId.Create(userId);
    }
}