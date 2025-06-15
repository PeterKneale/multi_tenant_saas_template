namespace Core.Application.Auth.Exceptions;

public class UserAlreadyExistsException : BusinessRuleBrokenException
{
    public UserAlreadyExistsException(UserId id) : base($"A user with id {id} already exists")
    {
    }

    public UserAlreadyExistsException(EmailAddress email) : base($"A user with email {email} already exists")
    {
    }
}