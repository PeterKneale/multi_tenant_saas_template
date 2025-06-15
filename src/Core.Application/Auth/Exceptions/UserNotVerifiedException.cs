namespace Core.Application.Auth.Exceptions;

public class UserNotVerifiedException(UserId id) :
    BusinessRuleBrokenException($"A user with id {id} is not verified");