namespace Web.IntegrationTests.UseCases.Auth;

public class AnonymousAccessTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    private readonly HttpClient _client = service.CreateDefaultClient();

    [Theory]
    [InlineData("/")]
    [InlineData("/Auth/Login")]
    [InlineData("/Auth/Register")]
    [InlineData("/Auth/Forbidden")]
    [InlineData("/Auth/Verify")]
    [InlineData("/Auth/Forgot")]
    [InlineData("/Auth/Reset")]
    [InlineData("/Error")]
    public async Task Can_navigate_to_anonymous_endpoint(string endpoint)
    {
        // arrange

        // act
        var response = await _client.GetAsync(endpoint);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}