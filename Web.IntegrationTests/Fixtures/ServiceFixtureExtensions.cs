using Core.Application.Auth.Commands;
using Core.Application.Users.Commands;
using Core.Application.Users.Queries;

namespace Web.IntegrationTests.Fixtures;

public static class ServiceFixtureExtensions
{
    public static async Task ProvisionOrganisation(this ServiceFixture fixture, Guid organisationId, Guid userId,
        string? email = null)
    {
        var title = Guid.NewGuid().ToString();
        email ??= "example" + Guid.NewGuid() + "@example.org";
        var first = Guid.NewGuid().ToString();
        var last = Guid.NewGuid().ToString();
        await fixture.Command(new Register.Command(organisationId, title, userId, first, last, email, "password"));

        var verification = await fixture.Query(new GetUserVerificationToken.Query(userId));

        await fixture.Command(new VerifyEmailAddress.Command(userId, verification));
    }

    public static async Task RegisterOrganisation(this ServiceFixture fixture, Guid organisationId, Guid userId,
        string? email = null)
    {
        var title = Guid.NewGuid().ToString();
        email ??= "example" + Guid.NewGuid() + "@example.org";
        var first = Guid.NewGuid().ToString();
        var last = Guid.NewGuid().ToString();
        await fixture.Command(new Register.Command(organisationId, title, userId, first, last, email, "password"));
    }
}