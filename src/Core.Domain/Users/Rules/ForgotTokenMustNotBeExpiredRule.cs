namespace Core.Domain.Users.Rules;

internal class ForgotTokenMustNotBeExpiredRule(User user) : IBusinessRule
{
    public string Message => "The forgot token has expired";

    public bool IsBroken()
    {
        return SystemTime.UtcNow() > user.ForgottenTokenExpiry;
    }
}