namespace Core.Domain.Users.Rules;

internal class MustNotBeActiveRule(User user) : IBusinessRule
{
    public string Message => "This action must be performed on an inactive user";

    public bool IsBroken()
    {
        return user.Active == true;
    }
}