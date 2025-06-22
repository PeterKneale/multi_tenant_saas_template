namespace Core.IntegrationTests.UseCases;

public class SetupMultipleOrganisations(ServiceFixture service, ITestOutputHelper output)
    : MultipleOrganisationTest(service, output)
{
    [Fact]
    public void Execute()
    {
    }
}