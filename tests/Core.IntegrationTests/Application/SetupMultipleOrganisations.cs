namespace Core.IntegrationTests.Application;

public class SetupMultipleOrganisations(ServiceFixture service, ITestOutputHelper output)
    : MultipleOrganisationTest(service, output)
{
    [Fact]
    public void Execute()
    {
    }
}