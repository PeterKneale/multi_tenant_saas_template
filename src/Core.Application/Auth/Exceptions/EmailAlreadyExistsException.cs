namespace Core.Application.Auth.Exceptions;

public class EmailAlreadyExistsException(EmailAddress email) :
    BusinessRuleBrokenException($"Email address {email} is in use");