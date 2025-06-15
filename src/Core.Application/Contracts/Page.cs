namespace Core.Application.Contracts;

public record Page(int PageNumber, int PageSize)
{
    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}