namespace Web.IntegrationTests.UseCases.Technical;

public class IconTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Theory]
    [InlineData("/favicon.ico")]
    [InlineData("/apple-touch-icon.png")]
    [InlineData("/apple-touch-icon-precomposed.png")]
    public async Task Can_get_icon(string path)
    {
        // arrange
        var client = Service.CreateDefaultClient();

        // act
        var response = await client.GetAsync(path);

        // assert
        response.EnsureSuccessStatusCode();
    }
}