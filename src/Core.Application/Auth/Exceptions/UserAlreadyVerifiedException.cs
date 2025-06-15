namespace Core.Application.Auth.Exceptions;

public class UserAlreadyVerifiedException(UserId id) :
    BusinessRuleBrokenException($"A user with id {id} already verified");