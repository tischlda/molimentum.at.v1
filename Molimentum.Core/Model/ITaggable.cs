using System.Collections.Generic;

namespace Molimentum.Model
{
    public interface ITaggable
    {
        ICollection<string> Tags { get; }
    }
}