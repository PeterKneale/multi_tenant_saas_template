namespace Web.Code.Authentication;

public class AuthResult
{
    private AuthResult(bool success, string? failureMessage = null)
    {
        IsSuccessful = success;
        FailureMessage = failureMessage;
    }

    public bool IsSuccessful { get; }
    public string? FailureMessage { get; }

    public static AuthResult Success()
    {
        return new AuthResult(true);
    }

    public static AuthResult Failure(string message)
    {
        return new AuthResult(false, message);
    }
}