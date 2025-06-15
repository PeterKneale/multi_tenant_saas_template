namespace Core.IntegrationTests.Application;

public class SetupOneOrganisation(ServiceFixture service, ITestOutputHelper output)
    : SingleOrganisationTest(service, output), IClassFixture<ServiceFixture>
{
    [Fact]
    public void Execute()
    {
    }
}