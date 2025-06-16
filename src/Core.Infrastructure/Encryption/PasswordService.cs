using Core.Domain.Users.Contracts;
using BC = BCrypt.Net.BCrypt;

namespace Core.Infrastructure.Encryption;

public class PasswordService : IPasswordCheck, IPasswordHash
{
    public bool Matches(string password, string hash) => BC.Verify(password, hash);

    public string HashPassword(string password) => BC.HashPassword(password);
}