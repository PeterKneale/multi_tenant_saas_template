using Core.Domain.Users.Contracts;

namespace Core.Infrastructure.Services;

public class PasswordService : IPasswordCheck, IPasswordHash
{
    public bool Matches(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}