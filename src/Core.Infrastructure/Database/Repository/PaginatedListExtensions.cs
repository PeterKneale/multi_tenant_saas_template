using Core.Application.Contracts;

namespace Core.Infrastructure.Database.Repository;

public static class PaginatedListExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int page,
        int pageSize)
    {
        var total = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, total, page, pageSize);
    }
}