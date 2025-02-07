namespace webapi.Models;

public class PaginatedList<T>
{
	public int PageIndex { get; private set; }
	public int TotalPages { get; private set; }

	public List<T> Data { get; private set; }

    public PaginatedList(List<T> items, int pageIndex, int totalPages)
	{
        PageIndex = pageIndex;
		TotalPages = totalPages;
        Data = new List<T>();
        Data.AddRange(items);
    }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
	{
		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);

		Data = new List<T>();
		Data.AddRange(items);
	}

	public bool HasPreviousPage => PageIndex > 1;

	public bool HasNextPage => PageIndex < TotalPages;

	public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
	{
		var count = source.Count();
		var totalPages = (int)Math.Ceiling(count / (double)pageSize);
		if (pageIndex > totalPages)
		{
			pageIndex = totalPages;
		}
		if (pageIndex <= 0)
		{
			pageIndex = 1;
		}
		var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}