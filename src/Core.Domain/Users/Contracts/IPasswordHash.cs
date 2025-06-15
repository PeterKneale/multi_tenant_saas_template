namespace Core.Domain.Users.Contracts;

public interface IPasswordHash
{
    string HashPassword(string password);
}