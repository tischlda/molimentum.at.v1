using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.NHibernate
{
    public class NHibernatePostCommentRepository : NHibernateRepositoryBase, IPostCommentRepository
    {
        public NHibernatePostCommentRepository(ISessionBuilder sessionBuilder)
            : base(sessionBuilder)
        {

        }

        public PagedList<PostComment> List(int page, int pageSize)
        {
            var postComments = Session.CreateQuery("from PostComment order by PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List()
                .Cast<PostComment>();

            var itemCount = Session.CreateQuery("select count(*) from PostComment").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostComment>(postComments, page, pageSize, pageCount);
        }

        public PagedList<PostComment> ListPublished(int page, int pageSize)
        {
            var postComments = Session.CreateQuery("from PostComment order by PublishDate desc")
                .SetFirstResult((page - 1) * pageSize)
                .SetMaxResults(pageSize)
                .List();

            var itemCount = Session.CreateQuery("select count(*) from PostComment").UniqueResult<Int64>();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostComment>(postComments.Cast<PostComment>(), page, pageSize, pageCount);
        }

        public PostComment Create()
        {
            var postComment = new PostComment { ID = Guid.NewGuid().ToString(), Author = "", Title = "", Body = "" };

            return postComment;
        }

        public PostComment Get(string id)
        {
            return Session.Get<PostComment>(id);
        }

        public void Save(PostComment item)
        {
            Session.Save(item);
        }

        public void Delete(PostComment item)
        {
            Session.Delete(item);
        }

        public void SubmitChanges()
        {
            Session.Flush();
        }
    }
}