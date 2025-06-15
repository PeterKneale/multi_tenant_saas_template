namespace Core.Application.Auth.Exceptions;

public class UserNotFoundException : BusinessRuleBrokenException
{
    public UserNotFoundException(UserId id) : base($"A user with id {id} not found")
    {
    }

    public UserNotFoundException(EmailAddress email) : base($"A user with email {email} not found")
    {
    }
}