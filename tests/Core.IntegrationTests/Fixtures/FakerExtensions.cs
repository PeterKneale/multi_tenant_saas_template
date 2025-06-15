namespace Core.IntegrationTests.Fixtures;

public static class FakerExtensions
{
    public static Register.Command ValidRegisterCommand(this Faker faker)
    {
        var firstName = faker.Name.FirstName();
        var lastName = faker.Name.LastName();
        return new Register.Command(
            Guid.NewGuid(),
            faker.Company.CompanyName() + faker.Random.Int(),
            Guid.NewGuid(),
            firstName,
            lastName,
            faker.Internet.Email(firstName, lastName),
            faker.Internet.Password(15)
        );
    }

    public static CreateInvitation.Command ValidInvitationCommand(this Faker fake, string? email = null)
    {
        return new CreateInvitation.Command(
            Guid.NewGuid(),
            email ?? fake.Internet.Email()
        );
    }

    public static AcceptInvitation.Command AcceptInvitationCommand(this Faker fake, Guid invitationId, string email)
    {
        return new AcceptInvitation.Command(
            invitationId,
            Guid.NewGuid(),
            fake.Name.FirstName(),
            fake.Name.LastName(),
            email,
            fake.Internet.Password()
        );
    }

    public static UpdateOrganisationName.Command UpdateOrganisationName()
    {
        return new Faker<UpdateOrganisationName.Command>()
            .CustomInstantiator(f =>
                new UpdateOrganisationName.Command(
                    f.OrganisationTitle(),
                    f.Company.CatchPhrase()
                )
            ).Generate();
    }

    private static string OrganisationTitle(this Faker faker)
    {
        return faker.Company.CompanyName() + faker.Random.Int();
        // Ensure a unique title;
    }
}