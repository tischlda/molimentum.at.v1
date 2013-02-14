using System;
using System.Collections.Generic;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryPostRepository : InMemoryRepositoryBase, IPostRepository
    {
        public InMemoryPostRepository(IStore store)
            : base(store)
        {

        }

        public PagedList<Post> List(int page, int pageSize)
        {
            var posts = (
                from post in Store.Posts.Values
                orderby post.PublishDate descending
                select post)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.Posts.Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<Post>(posts, page, pageSize, pageCount);
        }

        public PagedList<Post> ListPublished(int page, int pageSize)
        {
            var posts = (
                from post in Store.Posts.Values
                where post.IsPublished
                orderby post.PublishDate descending
                select post)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.Posts.Where(i => i.Value.IsPublished).Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<Post>(posts.Cast<Post>(), page, pageSize, pageCount);
        }

        public Post Create()
        {
            var post = new Post {ID = Guid.NewGuid().ToString() };

            return post;
        }

        public Post Get(string id)
        {
            return Store.Posts.ContainsKey(id) ? Store.Posts[id] : null;
        }

        public void Save(Post item)
        {
            Store.Posts.Add(item.ID, item);
        }

        public void Delete(Post item)
        {
            Store.Posts.Remove(item.ID);
        }

        public void SubmitChanges()
        {

        }

        public IEnumerable<TagSummary> ListTags()
        {
            return
                from post in Store.Posts.Values
                from tag in post.Tags
                orderby tag
                group tag by tag into groupedTags
                select new TagSummary { Tag = groupedTags.Key, Count = groupedTags.Count() };

            //var tags = Session.CreateQuery("select t, count(t) from Post p join p.Tags as t where t is not null and t != '' group by t")
            //    .List<object[]>();

            //return from t in tags
            //       select new TagSummary { Tag = (string)t[0], Count = (int)(Int64)t[1] };
        }

        public PagedList<Post> ListPublishedByTag(int page, int pageSize, string tag)
        {
            throw new NotImplementedException();

            //var posts = Session.CreateQuery("from Post p join p.Tags as t where t = :tag and p.IsPublished = 1 order by p.PublishDate desc")
            //    .SetFirstResult((page - 1) * pageSize)
            //    .SetMaxResults(pageSize)
            //    .SetParameter("tag", tag)
            //    .List();

            //var itemCount = Session.CreateQuery("select count(*) from Post p join p.Tags as t where t = :tag and p.IsPublished = 1")
            //                .SetParameter("tag", tag)
            //                .UniqueResult<Int64>();
            //var pageCount = Utils.CalculatePages(pageSize, itemCount);

            //return new PagedList<Post>(posts.Cast<Post>(), page, pageSize, pageCount);
        }

        public PagedList<Post> ListPublishedByCategory(int page, int pageSize, string category)
        {
            throw new NotImplementedException();

            //var posts = Session.CreateQuery("from Post p join p.PostCategory as pc where pc.Title = :category and p.IsPublished = 1 order by p.PublishDate desc")
            //    .SetFirstResult((page - 1) * pageSize)
            //    .SetMaxResults(pageSize)
            //    .SetParameter("category", category)
            //    .List();

            //var itemCount = Session.CreateQuery("select count(*) from Post p join p.PostCategory as pc where pc.Title = :category and p.IsPublished = 1")
            //                .SetParameter("category", category)
            //                .UniqueResult<Int64>();
            //var pageCount = Utils.CalculatePages(pageSize, itemCount);

            //return new PagedList<Post>(posts.Cast<Post>(), page, pageSize, pageCount);
        }
    }
}