using System.Diagnostics;

namespace Core.Domain.Organisations;

[DebuggerDisplay("{Title} ({Description})")]
public record OrganisationName(string Title, string? Description = null)
{
    public string Title { get; } = Title ?? throw new ArgumentNullException(nameof(Title), "title is null");
    public string? Description { get; } = Description;
}