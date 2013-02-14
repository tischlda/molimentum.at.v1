using System;
using System.Linq;
using System.Collections.Generic;

namespace Molimentum
{
    public static class QueryableExtension
    {
        public static PagedList<T> AsPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return new PagedList<T>(
                source.Skip((page - 1) * pageSize).Take(pageSize),
                page,
                pageSize,
                (int)Math.Ceiling((double)source.Count() / (double)pageSize));
        }

        public static PagedList<T> AsPagedList<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            return new PagedList<T>(
                source.Skip((page - 1) * pageSize).Take(pageSize),
                page,
                pageSize,
                (int)Math.Ceiling((double)source.Count() / (double)pageSize));
        }
    }
}