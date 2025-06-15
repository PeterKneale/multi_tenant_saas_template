namespace Core.IntegrationTests;

public abstract class SingleOrganisationTest(ServiceFixture service, ITestOutputHelper output)
    : BaseTest(service, output), IAsyncLifetime, IClassFixture<ServiceFixture>
{
    protected UserContext AdminUserContext { get; private set; } = null!;

    protected UserContext NonExistentUserContext =>
        new(Guid.NewGuid(), Guid.NewGuid(), Fake.Internet.Email(), Fake.Internet.Password());

    public async Task InitializeAsync()
    {
        Output.WriteLine("*** Test initialisation starting ***");
        AdminUserContext = await Provision();
        Output.WriteLine("*** Test initialisation complete ***");
        Output.WriteLine(
            $"Admin User Impersonation Link: http://localhost/test/Impersonate?email={AdminUserContext.Email}");
    }

    public Task DisposeAsync()
    {
        // No specific disposal logic needed for SingleOrganisationTest
        return Task.CompletedTask;
    }

    protected async Task CommandWithAdminContext(IRequest request)
    {
        await GetScope(AdminUserContext)
            .ServiceProvider
            .GetRequiredService<IMediator>()
            .Send(request);
    }

    protected async Task<TResponse> QueryWithAdminContext<TResponse>(IRequest<TResponse> request)
    {
        return await GetScope(AdminUserContext)
            .ServiceProvider
            .GetRequiredService<IMediator>()
            .Send(request);
    }
}