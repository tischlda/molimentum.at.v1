
namespace Molimentum.Web.Controllers
{
    [DefaultPageSize(10)]
    public abstract class ItemControllerBase : PrimaryControllerBase
    {
        protected ItemControllerBase()
        {
            var defaultPageSizeAttributes = (DefaultPageSizeAttribute[])GetType().GetCustomAttributes(typeof(DefaultPageSizeAttribute), true);

            DefaultPageSize = defaultPageSizeAttributes[0].PageSize;
        }

        public int DefaultPageSize
        {
            get; private set;
        }
    }
}
