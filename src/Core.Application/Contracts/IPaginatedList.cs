namespace Core.Application.Contracts;

public interface IPaginatedList<out T> : IEnumerable<T>
{
    int TotalPages { get; }
    IEnumerable<T> Items { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    int TotalItems { get; }
    int PageNumber { get; }
    int PageSize { get; }
}