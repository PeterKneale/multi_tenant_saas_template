using Core.Domain.Users.Contracts;

namespace Core.Domain.Users.Rules;

internal class PasswordMustMatchRule(User user, string password, IPasswordCheck check) : IBusinessRule
{
    public string Message => "The password does not match";

    public bool IsBroken()
    {
        return check.Matches(password, user.Password) == false;
    }
}