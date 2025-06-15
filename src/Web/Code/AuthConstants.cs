namespace Web.Code;

public static class AuthConstants
{
    public const int MinimumPasswordLength = 15;
    public const int MaximumPasswordLength = 50;

    public const string MinimumPasswordErrorMessage = "Your password must be at least {1} characters long.";
    public const string MaximumPasswordErrorMessage = "Your password must not exceed {1} characters.";
}