namespace Core.Domain.Users.Rules;

internal class MustNotBeVerifiedRule(User user) : IBusinessRule
{
    public string Message => "This user has already been verified";

    public bool IsBroken()
    {
        return user.Verified;
    }
}