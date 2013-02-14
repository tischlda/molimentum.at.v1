using System;
using System.Web;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Models;
using Molimentum;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    [DefaultPageSize(5)]
    public class PostsController : EditableItemControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;

        public PostsController(IPostRepository postRepository, IPostCategoryRepository postCategoryRepository)
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
        }

        public ActionResult Detail(string id)
        {
            var post = LoadPost(id);

            return View("Detail", post);
        }

        public ActionResult Print(string id)
        {
            var post = LoadPost(id);

            return View("Print", post);
        }

        public ActionResult Index(int? page, string mode)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var postListPage = HttpContext.User.IsInRole("Author")
                                   ? _postRepository.List(page.Value, pageSize)
                                   : _postRepository.ListPublished(page.Value, pageSize);

            if ("text".Equals(mode, StringComparison.InvariantCultureIgnoreCase))
            {
                return View("ListText", postListPage);
            }
            else
            {
                return View("List", postListPage);
            }
        }

        public ActionResult Query(int? page, string tag)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var postListPage = _postRepository.ListPublishedByTag(page.Value, pageSize, tag);

            return View("List", postListPage);
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Add()
        {
            var editData = new EditPostData
                               {
                                   EditMode = EditMode.Add,
                                   Post = _postRepository.Create(),
                                   PostCategories = _postCategoryRepository.List(1, int.MaxValue).Items
                               };

            return View("Edit", editData);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var editData = new EditPostData
                               {
                                   EditMode = EditMode.Edit,
                                   Post = LoadPost(id),
                                   PostCategories = _postCategoryRepository.List(1, int.MaxValue).Items
                               };

            return View("Edit", editData);
        }

        [Authorize(Roles = "Administrator,Author")]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(
            EditMode editMode, string id,
            string title, string body, bool isPublished, double? latitude, double? longitude,
            string categoryId,
            string newCategory,
            string tags,
            DateTime? publishDate,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            if (String.IsNullOrEmpty(title)) ModelState.AddModelError("title", "Title must not be empty.");

            var post = GetPost(editMode, id);

            post.Title = title;
            post.Body = body;
            post.LastUpdatedTime = DateTime.Now.ToUniversalTime();
            post.PublishDate = publishDate;
            post.IsPublished = isPublished;
            post.DateFrom = dateFrom;
            post.DateTo = dateTo;
            post.Position = latitude != null && longitude != null ? new Position(latitude.Value, longitude.Value) : null;
            post.Tags.Clear();
            foreach (var tag in tags.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries)) post.Tags.Add(tag);

            if(!String.IsNullOrEmpty(newCategory))
            {
                post.Category = CreatePostCategory(post, newCategory);
            }
            else
            {
                post.Category = GetPostCategory(categoryId);
            }

            if (!ModelState.IsValid)
            {
                var editData = new EditPostData
                {
                    EditMode = editMode,
                    Post = post,
                    PostCategories = _postCategoryRepository.List(1, int.MaxValue).Items
                };

                return View("Edit", editData);
            }

            if (editMode == EditMode.Add) _postRepository.Save(post);

            _postRepository.SubmitChanges();

            return RedirectToAction("Detail", new { id = post.ID });
        }

        private PostCategory GetPostCategory(string id)
        {
            return id == null ? null : _postCategoryRepository.Get(id);
        }

        private PostCategory CreatePostCategory(Post post, string title)
        {
            var postCategory = _postCategoryRepository.Create();
            
            postCategory.Title = title;

            _postCategoryRepository.Save(postCategory);
            
            return postCategory;
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Delete(string id)
        {
            var post = LoadPost(id);

            return View("ConfirmDelete", post);
        }

        [Authorize(Roles = "Administrator,Author")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDelete(string id)
        {
            var post = LoadPost(id);

            _postRepository.Delete(post);

            _postRepository.SubmitChanges();

            return RedirectToAction("Index");
        }

        private Post LoadPost(string id)
        {
            var post = _postRepository.Get(id);

            if (post == null) throw new HttpException(404, "Post not found.");

            return post;
        }

        private Post GetPost(EditMode editMode, string id)
        {
            switch (editMode)
            {
                case EditMode.Add:
                    return _postRepository.Create();

                case EditMode.Edit:
                    return _postRepository.Get(id);

                default:
                    throw new ArgumentException(String.Format("Unknown EditMode '{0}'.", editMode), "editMode");
            }
        }
    }
}