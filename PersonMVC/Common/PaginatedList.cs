﻿namespace PersonMVC.Common;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        AddRange(items);
    }

    public int PageIndex { get; }
    public int TotalPages { get; }
    public int PageSize { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(IList<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}