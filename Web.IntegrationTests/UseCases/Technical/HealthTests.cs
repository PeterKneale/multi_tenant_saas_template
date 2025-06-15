namespace Web.IntegrationTests.UseCases.Technical;

public class HealthTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Theory]
    [InlineData("/health/alive")]
    [InlineData("/health/ready")]
    public async Task Can_get_health_endpoint(string endpoint)
    {
        // arrange
        var client = Service.CreateDefaultClient();

        // act
        var response = await client.GetAsync(endpoint);

        // assert
        response.EnsureSuccessStatusCode();
    }
}