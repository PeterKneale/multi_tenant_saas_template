using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class InvitationIdConverter()
    : ValueConverter<InvitationId, Guid>(id => id.Value, guid => new InvitationId(guid));