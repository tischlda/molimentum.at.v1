namespace Molimentum.Web.Mvc
{
    public class ClientScriptIncludeAttribute : IncludeAttribute
    {
        public ClientScriptIncludeAttribute()
        {
            
        }

        public ClientScriptIncludeAttribute(params string[] urls)
        {
            Urls = urls;
        }
    }
}