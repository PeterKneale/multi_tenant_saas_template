namespace Web.IntegrationTests.UseCases.Technical;

public class RobotsTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Can_get_robots_txt()
    {
        // arrange
        var client = Service.CreateDefaultClient();
        var path = "/robots.txt";

        // act
        var response = await client.GetAsync(path);

        // assert
        response.EnsureSuccessStatusCode();
    }
}