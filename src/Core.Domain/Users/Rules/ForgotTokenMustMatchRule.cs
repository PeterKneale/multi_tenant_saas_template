namespace Core.Domain.Users.Rules;

internal class ForgotTokenMustMatchRule(User user, string token) : IBusinessRule
{
    public string Message => "The forgot token does not match";

    public bool IsBroken()
    {
        return user.ForgottenToken != token;
    }
}