using Core.Domain.Users;
using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

public static class BogusEntities
{
    public static User NewMember()
    {
        return new Faker<User>()
            .CustomInstantiator(f =>
                {
                    var organisationId = OrganisationId.Create(Guid.NewGuid());
                    var userId = UserId.Create(Guid.NewGuid());
                    var first = f.Name.FirstName();
                    var last = f.Name.LastName();
                    var name = new Name(first, last);
                    var email = EmailAddress.Create(f.Internet.ExampleEmail(first, last));
                    var password = f.Internet.Password();

                    return User.CreateMember(
                        organisationId,
                        userId,
                        name,
                        email,
                        password,
                        new Mock<IPasswordHash>().Object
                    );
                }
            ).Generate();
    }

    public static User NewOwner()
    {
        return new Faker<User>()
            .CustomInstantiator(f =>
                {
                    var organisationId = OrganisationId.Create(Guid.NewGuid());
                    var userId = UserId.Create(Guid.NewGuid());
                    var first = f.Name.FirstName();
                    var last = f.Name.LastName();
                    var name = new Name(first, last);
                    var email = EmailAddress.Create(f.Internet.ExampleEmail(first, last));
                    var password = f.Internet.Password();

                    return User.CreateOwner(
                        organisationId,
                        userId,
                        name,
                        email,
                        password,
                        new Mock<IPasswordHash>().Object
                    );
                }
            ).Generate();
    }
}