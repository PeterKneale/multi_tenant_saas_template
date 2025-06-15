using Core.Application.Contracts;
using Core.Domain.Users.Contracts;

namespace Core.Infrastructure.Services;

public class UserContext(ICurrentContext context) : IUserContext
{
    public UserId GetCurrentUserId()
    {
        return context.UserId;
    }
}