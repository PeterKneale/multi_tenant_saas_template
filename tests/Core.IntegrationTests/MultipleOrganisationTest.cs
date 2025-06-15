namespace Core.IntegrationTests;

public abstract class MultipleOrganisationTest(ServiceFixture service, ITestOutputHelper output)
    : BaseTest(service, output), IAsyncLifetime, IClassFixture<ServiceFixture>
{
    protected UserContext Org1Admin { get; private set; } = null!;
    protected UserContext Org2Admin { get; private set; } = null!;
    protected UserContext Org3Admin { get; private set; } = null!;
    protected UserContext Org1Member { get; private set; } = null!;
    protected UserContext Org2Member { get; private set; } = null!;
    protected UserContext Org3Member { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Output.WriteLine("*** Test initialisation starting ***");
        Org1Admin = await Provision();
        Org2Admin = await Provision();
        Org3Admin = await Provision();

        Org1Member = await ProvisionMember(Org1Admin);
        Org2Member = await ProvisionMember(Org2Admin);
        Org3Member = await ProvisionMember(Org3Admin);

        Output.WriteLine("*** Test initialisation complete ***");
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}