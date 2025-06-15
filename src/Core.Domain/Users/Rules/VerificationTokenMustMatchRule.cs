namespace Core.Domain.Users.Rules;

internal class VerificationTokenMustMatchRule(User user, string verification) : IBusinessRule
{
    public string Message => "The verification token does not match";

    public bool IsBroken()
    {
        return user.VerifiedToken != verification;
    }
}