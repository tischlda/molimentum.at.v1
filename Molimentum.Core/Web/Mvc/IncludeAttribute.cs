using System;

namespace Molimentum.Web.Mvc
{
    public abstract class IncludeAttribute : Attribute
    {
        public string[] Urls { get; set; }
    }
}