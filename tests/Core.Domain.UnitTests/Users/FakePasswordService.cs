using Core.Domain.Users.Contracts;

namespace Core.Domain.UnitTests.Users;

public class FakePasswordService : IPasswordCheck, IPasswordHash
{
    public bool Matches(string password, string hash)
    {
        return hash == password + "1";
    }

    public string HashPassword(string password)
    {
        return password + "1";
    }
}