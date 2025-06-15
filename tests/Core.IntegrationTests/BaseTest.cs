using Core.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Core.IntegrationTests;

//[Collection(nameof(ServiceFixtureCollection))]
public abstract class BaseTest
{
    protected readonly Faker Fake = new();

    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        Service = service;
        Output = output;
        Service.OutputHelper = Output;
    }

    protected ServiceFixture Service { get; }

    protected ITestOutputHelper Output { get; }

    protected async Task<UserContext> Provision()
    {
        var command = Fake.ValidRegisterCommand();
        await Command(command);
        await Command(new VerifyEmailAddressWithoutToken.Command(command.Email));

        return new UserContext(command.OrganisationId, command.UserId, command.Email, command.Password);
    }

    protected async Task<UserContext> ProvisionMember(UserContext context)
    {
        var invitation = Fake.ValidInvitationCommand();
        await Command(invitation, context);

        var acceptance = Fake.AcceptInvitationCommand(invitation.InvitationId, invitation.Email);
        await Command(acceptance);

        return new UserContext(context.OrganisationId, acceptance.UserId, acceptance.Email, acceptance.Password);
    }

    protected async Task Command(IRequest request, UserContext? userContext = null)
    {
        await GetScope(userContext)
            .ServiceProvider
            .GetRequiredService<IMediator>()
            .Send(request);
    }

    protected async Task<TResponse> Query<TResponse>(IRequest<TResponse> request, UserContext? userContext = null)
    {
        return await GetScope(userContext)
            .ServiceProvider
            .GetRequiredService<IMediator>()
            .Send(request);
    }

    protected AsyncServiceScope GetScope(UserContext? userContext = null)
    {
        var scope = Service.ServiceProvider.CreateAsyncScope();
        if (userContext == null) return scope;
        var context = scope.ServiceProvider.GetRequiredService<ICurrentContext>() as FakeCurrentContext
                      ?? throw new Exception("No context available");
        context.OrganisationId = new OrganisationId(userContext.OrganisationId);
        context.UserId = new UserId(userContext.UserId);
        return scope;
    }

    protected async Task ExecuteSql(string sql)
    {
        await using var scope = Service.ServiceProvider.CreateAsyncScope();
        await using var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        Output.WriteLine("Executing SQL: " + sql);
        await db.Database.ExecuteSqlRawAsync(sql);
    }
}