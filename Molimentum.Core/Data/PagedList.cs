using System.Collections.Generic;

namespace Molimentum.Data
{
    public class PagedList<T> : IPagedList
    {
        public PagedList(IEnumerable<T> items, int page, int pageSize, int pages)
        {
            Items = items;
            Page = page;
            Pages = pages;
            PageSize = pageSize;
        }

        public IEnumerable<T> Items { get; private set; }

        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int Pages { get; private set; }
    }
}