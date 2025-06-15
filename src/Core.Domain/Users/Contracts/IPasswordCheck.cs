namespace Core.Domain.Users.Contracts;

public interface IPasswordCheck
{
    bool Matches(string password, string hash);
}