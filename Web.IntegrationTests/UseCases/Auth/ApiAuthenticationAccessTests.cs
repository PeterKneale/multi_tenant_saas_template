using System.Net.Http.Json;

namespace Web.IntegrationTests.UseCases.Auth;

public class ApiAuthenticationAccessTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    private readonly HttpClient _client = service.CreateDefaultClient();

    [Fact]
    public async Task Apis_requires_authentication()
    {
        // arrange

        // act
        var response = await _client.PostAsJsonAsync("/api", new { });

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}