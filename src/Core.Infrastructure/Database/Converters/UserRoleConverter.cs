using Core.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class UserRoleConverter()
    : ValueConverter<UserRole, string>(role => role.Value, value => UserRole.Create(value));