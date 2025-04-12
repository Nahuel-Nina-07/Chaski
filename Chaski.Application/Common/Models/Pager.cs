namespace Chaski.Application.Common;

public class Pager<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    public int Skip => (PageIndex - 1) * PageSize;
    public int Take => PageSize;

    public Pager() {}

    public Pager(List<T> items, int totalItems, int pageIndex, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public static Pager<T> Create(List<T> items, int totalItems, int pageIndex, int pageSize) =>
        new(items, totalItems, pageIndex, pageSize);
}