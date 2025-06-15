namespace Core.Application.Auth.Exceptions;

public class UserNotActiveException(UserId id) :
    BusinessRuleBrokenException($"A user with id {id} is not active");