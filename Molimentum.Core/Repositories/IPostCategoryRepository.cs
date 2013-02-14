using Molimentum.Model;

namespace Molimentum.Repositories
{
    public interface IPostCategoryRepository : IItemRepository<PostCategory>
    {
        PostCategory GetByTitle(string title);
    }
}