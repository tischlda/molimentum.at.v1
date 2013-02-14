using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryPostCommentRepository : InMemoryRepositoryBase, IPostCommentRepository
    {
        public InMemoryPostCommentRepository(IStore store)
            : base(store)
        {

        }

        public PagedList<PostComment> List(int page, int pageSize)
        {
            var postComments = (
                from postComment in Store.PostComments.Values
                orderby postComment.PublishDate descending
                select postComment)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PostComments.Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostComment>(postComments, page, pageSize, pageCount);
        }

        public PagedList<PostComment> ListPublished(int page, int pageSize)
        {
            var postComments = (
                from postComment in Store.PostComments.Values
                orderby postComment.PublishDate descending
                select postComment)
                .Skip(pageSize * (page - 1)).Take(pageSize);

            var itemCount = Store.PostComments.Count();
            var pageCount = Utils.CalculatePages(pageSize, itemCount);

            return new PagedList<PostComment>(postComments, page, pageSize, pageCount);
        }

        public PostComment Create()
        {
            var postComment = new PostComment { ID = Guid.NewGuid().ToString(), Author = "", Title = "", Body = "" };

            return postComment;
        }

        public PostComment Get(string id)
        {
            return Store.PostComments.ContainsKey(id) ? Store.PostComments[id] : null;
        }

        public void Save(PostComment item)
        {
            Store.PostComments.Add(item.ID, item);
        }

        public void Delete(PostComment item)
        {
            Store.PostComments.Remove(item.ID);
        }

        public void SubmitChanges()
        {

        }
    }
}