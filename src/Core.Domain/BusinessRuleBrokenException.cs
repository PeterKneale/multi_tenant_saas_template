namespace Core.Domain;

[ExcludeFromCodeCoverage]
public class BusinessRuleBrokenException(string message) : Exception(message);