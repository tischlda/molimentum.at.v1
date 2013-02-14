namespace Molimentum.Web.Mvc
{
    public class StyleSheetIncludeAttribute : IncludeAttribute
    {
        public StyleSheetIncludeAttribute()
        {
            
        }

        public StyleSheetIncludeAttribute(params string[] urls)
        {
            Urls = urls;
        }
    }
}