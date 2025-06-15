using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class UserIdConverter()
    : ValueConverter<UserId, Guid>(id => id.Value, guid => new UserId(guid));