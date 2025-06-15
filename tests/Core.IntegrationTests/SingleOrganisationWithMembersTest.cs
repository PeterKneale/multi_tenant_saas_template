namespace Core.IntegrationTests;

public abstract class SingleOrganisationWithMembersTest(
    ServiceFixture service,
    ITestOutputHelper output,
    int memberCount) : BaseTest(service, output), IAsyncLifetime, IClassFixture<ServiceFixture>
{
    private readonly List<UserContext> _members = [];

    protected UserContext AdminUserContext { get; private set; }

    protected IEnumerable<UserContext> MemberUserContexts => _members;

    public async Task InitializeAsync()
    {
        Output.WriteLine("*** Test initialisation starting ***");
        AdminUserContext = await Provision();
        for (var i = 0; i < memberCount; i++) _members.Add(await ProvisionMember(AdminUserContext));

        Output.WriteLine("*** Test initialisation complete ***");
    }

    public Task DisposeAsync()
    {
        // No specific disposal logic needed for SingleOrganisationWithMembersTest
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