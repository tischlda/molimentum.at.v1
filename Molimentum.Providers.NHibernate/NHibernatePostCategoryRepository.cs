using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.NHibernate
{
    public class NHibernatePostCategoryRepository : NHibernateRepositoryBase, IPostCategoryRepository
    {
        public NHibernatePostCategoryRepository(ISessionBuilder sessionBuilder)
            : base(sessionBuilder)
        {

        }

        public PagedList<PostCategory> List(int page, int pageSize)
        {
            var postCategorys = Session.CreateQuery("from PostCategory order by Title")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List()
                .Cast<PostCategory>();

            var itemCount = Session.CreateQuery("select count(*) from PostCategory").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostCategory>(postCategorys, page, pageSize, pageCount);
        }

        public PagedList<PostCategory> ListPublished(int page, int pageSize)
        {
            var postCategorys = Session.CreateQuery("from PostCategory order by Title")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List();

            var itemCount = Session.CreateQuery("select count(*) from PostCategory").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostCategory>(postCategorys.Cast<PostCategory>(), page, pageSize, pageCount);
        }

        public PostCategory Create()
        {
            var postCategory = new PostCategory { ID = Guid.NewGuid().ToString(), Title = "", Body = "" };

            return postCategory;
        }

        public PostCategory Get(string id)
        {
            return Session.Get<PostCategory>(id);
        }

        public void Save(PostCategory item)
        {
            Session.Save(item);
        }

        public void Delete(PostCategory item)
        {
            Session.Delete(item);
        }

        public void SubmitChanges()
        {
            Session.Flush();
        }

        public PostCategory GetByTitle(string title)
        {
            return
                Session.CreateQuery("from PostCategory where title = :title")
                .SetParameter("title", title)
                .UniqueResult<PostCategory>();
        }
    }
}