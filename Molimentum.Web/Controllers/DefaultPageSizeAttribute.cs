using System;

namespace Molimentum.Web.Controllers
{
    public class DefaultPageSizeAttribute : Attribute
    {
        public DefaultPageSizeAttribute(int pageSize)
        {
            PageSize = pageSize;
        }

        public int PageSize { get; set; }
    }
}
