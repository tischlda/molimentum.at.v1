using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Models;
using Molimentum.Services;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class PostCommentsController : EditableItemControllerBase
    {
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IPostRepository _postRepository;
        private readonly INotificationService _notificationService;

        public PostCommentsController(IPostRepository postRepository, IPostCommentRepository postCommentRepository,
            INotificationService notificationService)
        {
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _notificationService = notificationService;
        }

        public ActionResult Index(int? page, int? pageSize, string mode)
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = DefaultPageSize;

            var postCommentListPage = _postCommentRepository.ListPublished(page.Value, pageSize.Value);

            if ("text".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("ListText", postCommentListPage);
            }
            else
            {
                return View("List", postCommentListPage);
            }
        }

        public ActionResult ListForPost(string postID, int? page, int? pageSize, string mode)
        {
            if (page == null) page = 1;
            if (pageSize == null) pageSize = DefaultPageSize;
            
            var post = LoadPost(postID);

            var postCommentListPage = post.Comments.AsPagedList(page.Value, pageSize.Value);

            if ("text".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("ListText", postCommentListPage);
            }
            else
            {
                return View("List", postCommentListPage);
            }
        }

        public ActionResult Add(string postId)
        {
            var postComment = GetPostComment(EditMode.Add, null, postId);

            var editData = new EditPostCommentData
                               {
                                   EditMode = EditMode.Add,
                                   PostComment = postComment
                               };

            return View("Edit", editData);
        }

        public ActionResult Reply(string id)
        {
            var originalPostComment = LoadPostComment(id);
            var postComment = GetPostComment(EditMode.Add, null, originalPostComment.Post != null ? originalPostComment.Post.ID : null);
            postComment.Title = "Re: " + originalPostComment.Title;
            postComment.Body = Utils.QuoteBody(originalPostComment.Body);

            var editData = new EditPostCommentData
            {
                EditMode = EditMode.Add,
                PostComment = postComment
            };

            return View("Edit", editData);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var editData = new EditPostCommentData
                           {
                               EditMode = EditMode.Edit,
                               PostComment = LoadPostComment(id)
                           };

            return View("Edit", editData);
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(
            EditMode editMode, string id, string postId, string shipName)
        {
            if (String.IsNullOrEmpty(shipName) ||
                !String.Equals(shipName, "Molimentum", StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("shipName", "Der Name des Schiffes ist falsch.");
            }

            var postComment = GetPostComment(editMode, id, postId);

            TryUpdateModel(postComment, "PostComment", new [] { "Author", "Title", "Body", "Email", "Website", "PublishDate" } );

            if (!ModelState.IsValid)
            {
                var editData = new EditPostCommentData
                {
                    EditMode = editMode,
                    PostComment = postComment
                };

                return View("Edit", editData);
            }
            
            postComment.PublishDate = DateTime.Now;
            
            if (editMode == EditMode.Add) _postCommentRepository.Save(postComment);

            _postCommentRepository.SubmitChanges();
            
            if (editMode == EditMode.Add) _notificationService.Notify("New Comment", postComment);

            if (postComment.Post != null)
            {
                return RedirectToAction("Detail", "Posts", new { id = postComment.Post.ID });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private PostComment GetPostComment(EditMode editMode, string id, string postId)
        {
            switch (editMode)
            {
                case EditMode.Add:
                    var post = postId != null ? LoadPost(postId) : null;
                    var postComment = _postCommentRepository.Create();
                    postComment.Post = post;
                    return postComment;

                case EditMode.Edit:
                    return _postCommentRepository.Get(id);

                default:
                    throw new ArgumentException(String.Format("Unknown EditMode '{0}'.", editMode), "editMode");
            }
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Delete(string id)
        {
            var comment = LoadPostComment(id);

            return View("ConfirmDelete", comment);
        }

        [Authorize(Roles = "Administrator,Author")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDelete(string id)
        {
            var comment = LoadPostComment(id);
            var post = comment.Post;

            _postCommentRepository.Delete(comment);

            _postCommentRepository.SubmitChanges();

            if (post != null)
            {
                return RedirectToAction("Detail", "Posts", new { id = post.ID });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        private string Sanitize(string s, int maxLength)
        {
            if (s == null) return s;

            s = WebUtility.HtmlEncode(s);
            if (s.Length > maxLength) s = s.Substring(0, maxLength);

            return s;
        }

        private Post LoadPost(string id)
        {
            var post = _postRepository.Get(id);

            if (post == null) throw new HttpException(404, "Post not found.");

            return post;
        }

        private PostComment LoadPostComment(string id)
        {
            var comment = _postCommentRepository.Get(id);

            if (comment == null) throw new HttpException(404, "PostComment not found.");

            return comment;
        }
    }
}