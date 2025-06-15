namespace Core.Domain.Users.Rules;

internal class MustBeActiveRule(User user) : IBusinessRule
{
    public string Message => "This action must be performed on an active user";

    public bool IsBroken()
    {
        return user.Active is null or false;
    }
}