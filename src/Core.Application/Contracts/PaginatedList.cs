using System.Collections;

namespace Core.Application.Contracts;

public class PaginatedList<T> : IPaginatedList<T>
{
    public PaginatedList(IEnumerable<T> items, int total, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = total;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
    }

    public int TotalPages { get; }

    public IEnumerable<T> Items { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public int TotalItems { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}