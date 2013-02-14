using System.Collections.Generic;
using Molimentum.Model;

namespace Molimentum.Repositories
{
    public interface IPostRepository : IItemRepository<Post>
    {
        IEnumerable<TagSummary> ListTags();

        PagedList<Post> ListPublishedByTag(int page, int pageSize, string tag);

        PagedList<Post> ListPublishedByCategory(int page, int pageSize, string category);
    }
}