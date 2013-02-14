using System;

namespace Molimentum.Model
{
    public class SitemapUrl
    {
        public object Action { get;  set; }
        public DateTime? LastModified { get;  set; }
        public SiteMapChangeFrequency ChangeFrequency { get;  set; }
        public Single Priority { get; set; }
    }
}