using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users.Fakes;

public class FakeUserContext : IUserContext
{
    private readonly UserId _userId;

    private FakeUserContext(UserId userId)
    {
        _userId = userId;
    }

    public UserId GetCurrentUserId()
    {
        return _userId;
    }

    /// A user context with a random user id
    public static FakeUserContext WithRandomUserId()
    {
        return new FakeUserContext(UserId.Create(Guid.NewGuid()));
    }

    /// A user context with a specific user id
    public static FakeUserContext WithUserId(UserId userId)
    {
        return new FakeUserContext(userId);
    }
}