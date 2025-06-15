namespace Web.IntegrationTests.UseCases.Auth;

public class WebAuthenticationTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    private readonly HttpClient _client = service.CreateDefaultClient();

    [Theory]
    [InlineData("/admin")]
    public async Task Pages_requires_authentication(string endpoint)
    {
        // arrange

        // act
        var response = await _client.GetAsync(endpoint);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location!.PathAndQuery.Should().Contain("/Auth/Login");
    }
}