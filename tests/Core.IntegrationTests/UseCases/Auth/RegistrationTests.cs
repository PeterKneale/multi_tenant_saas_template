using Core.Application.Organisations.Exceptions;

namespace Core.IntegrationTests.UseCases.Auth;

public class RegistrationTests(ServiceFixture service, ITestOutputHelper output)
    : BaseTest(service, output), IClassFixture<ServiceFixture>
{
    [Fact]
    public async Task Can_register()
    {
        // arrange
        var command = Fake.ValidRegisterCommand();
        var context = new UserContext(command.OrganisationId, command.UserId, command.Email, command.Password);

        // act
        await Command(command);

        // assert
        var organisation = await Query(new GetOrganisation.Query(), context);
        organisation.Id.Should().Be(command.OrganisationId);
        organisation.Title.Should().Be(command.Title);
        organisation.Description.Should().BeNull();

        var user = await Query(new GetUser.Query(command.UserId), context);
        user.Id.Should().Be(command.UserId);
        user.FirstName.Should().Be(command.FirstName);
        user.LastName.Should().Be(command.LastName);
        user.Email.Should().Be(command.Email.ToLowerInvariant());
        user.Role.Should().BeEquivalentTo(UserRole.OwnerRoleName);
        user.Verified.Should().BeFalse("Not yet verified");
        user.Active.Should().BeNull("Not yet verified, so neither active nor inactive");
    }

    [Fact]
    public async Task Cant_register_with_same_organisation_name()
    {
        // arrange
        var title = Guid.NewGuid().ToString();
        var command1 = Fake.ValidRegisterCommand() with
        {
            Title = title
        };
        var command2 = Fake.ValidRegisterCommand() with
        {
            Title = title
        };

        // act
        await Command(command1);
        var act = async () => await Command(command2);

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>().WithMessage("Organisation name is used");
    }

    [Fact]
    public async Task Cant_register_with_same_email_address()
    {
        // arrange
        var email = Fake.Internet.Email();
        var command1 = Fake.ValidRegisterCommand() with
        {
            Email = email
        };
        var command2 = Fake.ValidRegisterCommand() with
        {
            Email = email
        };

        // act
        await Command(command1);
        var act = async () => await Command(command2);

        // assert
        await act.Should().ThrowAsync<EmailAlreadyExistsException>();
    }

    [Fact]
    public async Task Cant_register_with_same_organisation_id()
    {
        // arrange
        var command1 = Fake.ValidRegisterCommand();
        var command2 = Fake.ValidRegisterCommand() with
        {
            OrganisationId = command1.OrganisationId
        };

        // act
        await Command(command1);
        var act = async () => await Command(command2);

        // assert
        await act.Should().ThrowAsync<OrganisationAlreadyExistsException>();
    }

    [Fact]
    public async Task Cant_register_with_same_user_id()
    {
        // arrange
        var command1 = Fake.ValidRegisterCommand();
        var command2 = Fake.ValidRegisterCommand() with
        {
            UserId = command1.UserId
        };

        // act
        await Command(command1);
        var act = async () => await Command(command2);

        // assert
        await act.Should().ThrowAsync<UserAlreadyExistsException>();
    }
}