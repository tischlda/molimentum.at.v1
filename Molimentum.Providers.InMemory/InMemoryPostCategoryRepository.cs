using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryPostCategoryRepository : InMemoryRepositoryBase, IPostCategoryRepository
    {
        public InMemoryPostCategoryRepository(IStore store)
            : base(store)
        {

        }

        public PagedList<PostCategory> List(int page, int pageSize)
        {
            var postCategories = (
                from postCategory in Store.PostCategories.Values
                orderby postCategory.Title ascending
                select postCategory)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PostCategories.Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostCategory>(postCategories, page, pageSize, pageCount);
        }

        public PagedList<PostCategory> ListPublished(int page, int pageSize)
        {
            var postCategories = (
                from postCategory in Store.PostCategories.Values
                orderby postCategory.Title ascending
                select postCategory)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PostCategories.Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostCategory>(postCategories, page, pageSize, pageCount);
        }

        public PostCategory Create()
        {
            var postCategory = new PostCategory { ID = Guid.NewGuid().ToString(), Title = "", Body = "" };

            return postCategory;
        }

        public PostCategory Get(string id)
        {
            return Store.PostCategories.ContainsKey(id) ? Store.PostCategories[id] : null;
        }

        public void Save(PostCategory item)
        {
            Store.PostCategories.Add(item.ID, item);
        }

        public void Delete(PostCategory item)
        {
            Store.PostCategories.Remove(item.ID);
        }

        public void SubmitChanges()
        {

        }

        public PostCategory GetByTitle(string title)
        {
            return (
                from postCategory in Store.PostCategories.Values
                where postCategory.Title == title
                select postCategory)
                .FirstOrDefault();
        }
    }
}