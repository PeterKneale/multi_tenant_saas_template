using Core.Domain.Users.Contracts;

namespace Core.Domain.Users.Rules;

internal class MustNotBeSelfRule(User user, IUserContext context) : IBusinessRule
{
    public string Message => "This action can only be performed on others";

    public bool IsBroken()
    {
        return user.Id.Equals(context.GetCurrentUserId());
    }
}