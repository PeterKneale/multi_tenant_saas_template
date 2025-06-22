using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class ProjectIdConverter()
    : ValueConverter<ProjectId, Guid>(id => id.Value, guid => new ProjectId(guid));