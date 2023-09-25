namespace GeliosFill.Api.AppServices;

public static class PaginationHelper
{
    public static Task<List<T>> PaginateAsync<T>(List<T> queryable, int pageIndex, int pageSize=10)
    {
        if (queryable == null)
            throw new ArgumentNullException(nameof(queryable));

        if (pageIndex < 1)
            throw new ArgumentException("Page index must be greater than or equal to 1.", nameof(pageIndex));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than or equal to 1.", nameof(pageSize));

        var totalCount = queryable.Count();
        var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

        if (pageIndex > pageCount)
            throw new ArgumentException("Page index is out of range.", nameof(pageIndex));

        var items = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(items);
    }
}