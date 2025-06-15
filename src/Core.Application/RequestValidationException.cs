namespace Core.Application;

public class RequestValidationException(string message) : BusinessRuleBrokenException(message);