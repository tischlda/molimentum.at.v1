using System;
using System.Collections.Generic;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.NHibernate
{
    public class NHibernatePostRepository : NHibernateRepositoryBase, IPostRepository
    {
        public NHibernatePostRepository(ISessionBuilder sessionBuilder)
            : base(sessionBuilder)
        {

        }

        public PagedList<Post> List(int page, int pageSize)
        {
            var posts = Session.CreateQuery("from Post order by PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List()
                .Cast<Post>();

            var itemCount = Session.CreateQuery("select count(*) from Post").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<Post>(posts, page, pageSize, pageCount);
        }

        public PagedList<Post> ListPublished(int page, int pageSize)
        {
            var posts = Session.CreateQuery("from Post where IsPublished = 1 order by PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List();

            var itemCount = Session.CreateQuery("select count(*) from Post where IsPublished = 1").UniqueResult<Int64>();
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
            return Session.Get<Post>(id);
        }

        public void Save(Post item)
        {
            Session.Save(item);
        }

        public void Delete(Post item)
        {
            Session.Delete(item);
        }

        public void SubmitChanges()
        {
            Session.Flush();
        }

        public IEnumerable<TagSummary> ListTags()
        {
            var tags = Session.CreateQuery("select t, count(t) from Post p join p.Tags as t where t is not null and t != '' group by t")
                .List<object[]>();

            return from t in tags
                   select new TagSummary { Tag = (string)t[0], Count = (int)(Int64)t[1] };
        }

        public PagedList<Post> ListPublishedByTag(int page, int pageSize, string tag)
        {
            var posts = Session.CreateQuery("from Post p join p.Tags as t where t = :tag and p.IsPublished = 1 order by p.PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .SetParameter("tag", tag)
                .List();

            var itemCount = Session.CreateQuery("select count(*) from Post p join p.Tags as t where t = :tag and p.IsPublished = 1")
                            .SetParameter("tag", tag)
                            .UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<Post>(posts.Cast<Post>(), page, pageSize, pageCount);
        }

        public PagedList<Post> ListPublishedByCategory(int page, int pageSize, string category)
        {
            var posts = Session.CreateQuery("from Post p join p.PostCategory as pc where pc.Title = :category and p.IsPublished = 1 order by p.PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .SetParameter("category", category)
                .List();

            var itemCount = Session.CreateQuery("select count(*) from Post p join p.PostCategory as pc where pc.Title = :category and p.IsPublished = 1")
                            .SetParameter("category", category)
                            .UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<Post>(posts.Cast<Post>(), page, pageSize, pageCount);
        }
    }
}