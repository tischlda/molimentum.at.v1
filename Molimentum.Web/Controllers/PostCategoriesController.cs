using System;
using System.Web;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Models;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class PostCategoriesController : EditableItemControllerBase
    {
        private readonly IPostCategoryRepository _postCategoryRepository;

        public PostCategoriesController(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }

        private PostCategory LoadPostCategory(string id)
        {
            var postCategory = _postCategoryRepository.Get(id);

            if (postCategory == null) throw new HttpException(404, "PostCategory not found.");

            return postCategory;
        }

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var postCategoryListPage = _postCategoryRepository.List(page.Value, pageSize);

            return View("List", postCategoryListPage);
        }

        [Authorize]
        public ActionResult Add()
        {
            var editData = new EditPostCategoryData
                               {
                                   EditMode = EditMode.Add,
                                   PostCategory = _postCategoryRepository.Create()
                               };

            return View("Edit", editData);
        }

        public ActionResult Detail(string id)
        {
            var postCategory = LoadPostCategory(id);

            return View("Detail", postCategory);
        }

        public ActionResult DetailByTitle(string title)
        {
            var postCategory = _postCategoryRepository.GetByTitle(title);

            return View("Detail", postCategory);
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Edit(string id)
        {
            var editData = new EditPostCategoryData
                               {
                                   EditMode = EditMode.Edit,
                                   PostCategory = LoadPostCategory(id)
                               };

            return View("Edit", editData);
        }

        [Authorize(Roles = "Administrator,Author")]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(
            EditMode editMode, string id,
            string title, string body, PostListOrder postListOrder)
        {
            if (String.IsNullOrEmpty(title)) ModelState.AddModelError("title", "Title must not be empty.");

            var postCategory = GetPostCategory(editMode, id);

            postCategory.Title = title;
            postCategory.Body = body;
            postCategory.PostListOrder = postListOrder;

            if (!ModelState.IsValid)
            {
                var editData = new EditPostCategoryData
                {
                    EditMode = editMode,
                    PostCategory = postCategory
                };

                return View("Edit", editData);
            }

            if (editMode == EditMode.Add) _postCategoryRepository.Save(postCategory);

            _postCategoryRepository.SubmitChanges();

            return RedirectToAction("Detail", new { id = postCategory.ID });
        }

        private PostCategory GetPostCategory(EditMode editMode, string id)
        {
            PostCategory postCategory;

            switch (editMode)
            {
                case EditMode.Add:
                    postCategory = _postCategoryRepository.Create();
                    break;

                case EditMode.Edit:
                    postCategory = _postCategoryRepository.Get(id);
                    break;

                default:
                    throw new ArgumentException(String.Format("Unknown EditMode '{0}'.", editMode), "editMode");
            }

            return postCategory;
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Delete(string id)
        {
            var postCategory = LoadPostCategory(id);

            return View("ConfirmDelete", postCategory);
        }

        [Authorize(Roles = "Administrator,Author")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDelete(string id)
        {
            var postCategory = LoadPostCategory(id);

            _postCategoryRepository.Delete(postCategory);

            _postCategoryRepository.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}