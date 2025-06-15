namespace Core.Domain.Users.Contracts;

public interface IUserContext
{
    UserId GetCurrentUserId();
}