using System;
using System.Web;
using System.Web.Mvc;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using Molimentum.Web.Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class PostsControllerFixture : ControllerFixtureBase<PostsController>
    {
        private IPostRepository _postRepositoryMock;
        private IPostCategoryRepository _postCategoryRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postRepositoryMock = MockRepository.StrictMock<IPostRepository>();
            _postCategoryRepositoryMock = MockRepository.StrictMock<IPostCategoryRepository>();
        }

        
        protected override PostsController CreateTestedController()
        {
            return new PostsController(_postRepositoryMock, _postCategoryRepositoryMock);
        }



        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;

            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(5));
        }


        [Test]
        public void DetailTest()
        {
            var postID = "TestPostID";

            var post = new Post();
            post.ID = postID;


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);


            var result = TestedController.Detail(postID);


            VerifyViewResult(result, "Detail", typeof(Post), post);
        }

        [Test]
        public void DetailNotFoundTest()
        {
            var postID = "TestPostID";


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(null);


            try
            {
                var result = TestedController.Detail(postID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        [Test, Sequential]
        public void IndexTest(
            [Values(10, 10, 10, 10, 10)] int pages,
            [Values(null, 10, 12, null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage,
            [Values(false, false, false, true, true, true)] bool asAuthor,
            [Values("text", null, null, null, null, null)] string mode,
            [Values("ListText", "List", "List", "List", "List", "List")] string expectedView)
        {
            var expectedPageSize = TestedController.DefaultPageSize;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var postListPage = new PagedList<Post>(new Post[] { }, expectedPage, expectedPageSize, pages);

            if (asAuthor) AddRole("Author");

            if (asAuthor)
            {
                _postRepositoryMock.Expect(r => r.List(expectedRequestedPage, expectedPageSize)).Return(postListPage);
            }
            else
            {
                _postRepositoryMock.Expect(r => r.ListPublished(expectedRequestedPage, expectedPageSize)).Return(postListPage);
            }


            var result = TestedController.Index(requestedPage, mode);


            VerifyViewResult(result, expectedView, typeof(PagedList<Post>), postListPage);
        }


        [Test, Sequential]
        public void QueryTest(
            [Values(10, 10, 10)] int pages,
            [Values(null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10)] int expectedPage)
        {
            var tag = "Tag1";
            var expectedPageSize = TestedController.DefaultPageSize;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var postListPage = new PagedList<Post>(new Post[] { }, expectedPage, expectedPageSize, pages);

            _postRepositoryMock.Expect(r => r.ListPublishedByTag(expectedRequestedPage, expectedPageSize, tag)).Return(postListPage);


            var result = TestedController.Query(requestedPage, tag);


            VerifyViewResult(result, "List", typeof(PagedList<Post>), postListPage);
        }


        [Test]
        public void PrintTest()
        {
            var postID = "TestPostID";

            var post = new Post();
            post.ID = postID;


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);


            var result = TestedController.Print(postID);


            VerifyViewResult(result, "Print", typeof(Post), post);
        }

        
        [Test]
        public void AddTest()
        {
            var postID = Guid.Empty.ToString();

            var post = new Post();
            post.ID = postID;

            var postCategoryList = new PagedList<PostCategory>(new PostCategory[] { }, 1, 10, 1);

            AddRole("Author");
            _postRepositoryMock.Expect(r => r.Create()).Return(post);
            _postCategoryRepositoryMock.Expect(r => r.List(1, int.MaxValue)).Return(postCategoryList);


            var result = TestedController.Add();


            VerifyViewResult(result, "Edit", typeof(EditPostData));
            var resultModel = (EditPostData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.Post, Is.EqualTo(post));
            Assert.That(resultModel.PostCategories, Is.SameAs(postCategoryList.Items));
        }


        [Test]
        public void EditTest()
        {
            var postID = "TestPostID";

            var post = new Post();
            post.ID = postID;

            var postCategoryList = new PagedList<PostCategory>(new PostCategory[] { }, 1, 10, 1);

            AddRole("Author");
            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            _postCategoryRepositoryMock.Expect(r => r.List(1, int.MaxValue)).Return(postCategoryList);


            var result = TestedController.Edit(postID);


            VerifyViewResult(result, "Edit", typeof(EditPostData));
            var resultModel = (EditPostData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(resultModel.Post, Is.EqualTo(post));
            Assert.That(resultModel.PostCategories, Is.SameAs(postCategoryList.Items));
        }


        [Test]
        public void EditNotFoundTest()
        {
            var postID = "TestPostID";

            AddRole("Author");
            _postRepositoryMock.Expect(r => r.Get(postID)).Return(null);
  

            try
            {
                var result = TestedController.Edit(postID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        [Test, Sequential]
        public void SaveNewTest(
            [Values(false, true)] bool isPublished,
            [Values("1", null)] string categoryID,
            [Values(null, "2")] string newCategory,
            [Values(15.0, null)] double? latitude,
            [Values(null, 42.0)] double? longitude,
            [Values(false, true)] bool setPublishDate,
            [Values(false, true)] bool setDateFrom,
            [Values(false, true)] bool setDateTo)
        {
            var editMode = EditMode.Add;

            var postID = Guid.Empty.ToString();
            var title = "A post title";
            var body = string.Empty;
            var tags = new[] { "Tag1", "Tag2", "Tag3" };
            var publishDate = setPublishDate ? new DateTime?(new DateTime(2001, 1, 1)) : null;
            var dateFrom = setDateFrom ? new DateTime?(new DateTime(2001, 1, 2)) : null;
            var dateTo = setDateTo ? new DateTime?(new DateTime(2001, 1, 3)) : null;
            var position = latitude != null && longitude != null ? new Position(latitude.Value, longitude.Value) : null;

            var post = new Post();
            post.ID = postID;


            var category = new PostCategory();

            if (!String.IsNullOrEmpty(newCategory))
            {
                _postCategoryRepositoryMock.Expect(r => r.Create()).Return(category);
                _postCategoryRepositoryMock.Expect(
                    r => r.Save(category))
                .WhenCalled(
                    p => Assert.That(category.Title, Is.EqualTo(newCategory)));
            }
            else
            {
                category.ID = categoryID;
                _postCategoryRepositoryMock.Expect(r => r.Get(categoryID)).Return(category);
            }

            _postRepositoryMock.Expect(r => r.Create()).Return(post);
            _postRepositoryMock.Expect(r => r.Save(post));
            _postRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPost(post, postID, title, body, isPublished, position, category, tags, publishDate, dateFrom, dateTo));


            var result = TestedController.Save(editMode, postID, title, body, isPublished, latitude, longitude, categoryID, newCategory, tags.Concat(";"), publishDate, dateFrom, dateTo);


            VerifyRedirectToRouteResult(result, expectedAction: "Detail", expectedID: postID);
        }


        [Test, Sequential]
        public void SaveExistingTest(
            [Values(false, true)] bool isPublished,
            [Values("1", null)] string categoryID,
            [Values(null, "2")] string newCategory,
            [Values(42.0, null)] double? latitude,
            [Values(15.0, null)] double? longitude,
            [Values(false, true)] bool setPublishDate,
            [Values(false, true)] bool setDateFrom,
            [Values(false, true)] bool setDateTo)
        {
            var editMode = EditMode.Edit;

            var postID = "TestPostID";
            var title = "A post title";
            var body = string.Empty;
            var tags = new string[] { "Tag1", "Tag2", "Tag3" };
            var publishDate = setPublishDate ? new DateTime?(new DateTime(2001, 1, 1)) : null;
            var dateFrom = setDateFrom ? new DateTime?(new DateTime(2001, 1, 2)) : null;
            var dateTo = setDateTo ? new DateTime?(new DateTime(2001, 1, 3)) : null;
            var position = latitude != null && longitude != null ? new Position(latitude.Value, longitude.Value) : null;


            var post = new Post();
            post.ID = postID;
            post.Title = "oldTitle";
            post.Body = "oldBody";
            post.Position = new Position(41, 14);
            post.Tags.Add("Tag2");
            post.Tags.Add("Tag4");


            var category = MockRepository.GenerateStub<PostCategory>();

            if (!String.IsNullOrEmpty(newCategory))
            {
                _postCategoryRepositoryMock.Expect(r => r.Create()).Return(category);
                _postCategoryRepositoryMock.Expect(
                    r => r.Save(category))
                .WhenCalled(
                    p => Assert.That(category.Title, Is.EqualTo(newCategory)));
            }
            else
            {
                category.ID = categoryID;
                _postCategoryRepositoryMock.Expect(r => r.Get(categoryID)).Return(category);
            }

            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            _postRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPost(post, postID, title, body, isPublished, position, category, tags, publishDate, dateFrom, dateTo));


            var result = TestedController.Save(editMode, postID, title, body, isPublished, latitude, longitude, categoryID, newCategory, tags.Concat(";"), publishDate, dateFrom, dateTo);


            VerifyRedirectToRouteResult(result, expectedAction: "Detail", expectedID: postID);
        }


        [Test]
        public void SaveInvalidTest(
            [Values("", null)] string title)
        {
            var editMode = EditMode.Add;

            var postID = Guid.Empty.ToString();
            var body = string.Empty;
            var tags = new[] { "Tag1", "Tag2", "Tag3" };

            var postCategoryList = new PagedList<PostCategory>(new PostCategory[] { }, 1, 10, 1);
            _postCategoryRepositoryMock.Expect(r => r.List(1, int.MaxValue)).Return(postCategoryList);

            var post = new Post();
            post.ID = postID;

            _postRepositoryMock.Expect(r => r.Create()).Return(post);


            var result = TestedController.Save(editMode, postID, title, body, false, null, null, null, null, tags.Concat(";"), null, null, null);


            VerifyViewResult(result, "Edit", typeof(EditPostData));

            var viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.IsValidField("title"), Is.False);
            Assert.That(viewResult.ViewData.ModelState.IsValidField("body"), Is.True);

            var resultModel = (EditPostData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.Post, Is.EqualTo(post));
            Assert.That(resultModel.PostCategories, Is.SameAs(postCategoryList.Items));
        }


        [Test]
        public void SaveWithInvalidEditModeTest()
        {
            var editMode = (EditMode)Int32.MaxValue;

            var postID = Guid.Empty.ToString();
            var title = "A post title";
            var body = string.Empty;
            var tags = new[] { "Tag1", "Tag2", "Tag3" };

            var post = new Post();
            post.ID = postID;


            try
            {
                var result = TestedController.Save(editMode, postID, title, body, false, null, null, null, null, tags.Concat(";"), null, null, null);
                
                Assert.Fail("ArgumentException not raised.");
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("editMode"));
            }
        }

        
        [Test]
        public void DeleteTest()
        {
            var postID = "TestPostID";

            var post = new Post();
            post.ID = postID;


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);


            var result = TestedController.Delete(postID);


            VerifyViewResult(result, "ConfirmDelete", typeof(Post), post);
        }


        [Test]
        public void SaveDeleteTest()
        {
            var postID = "TestPostID";

            var post = new Post();
            post.ID = postID;


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            _postRepositoryMock.Expect(r => r.Delete(post));
            _postRepositoryMock.Expect(r => r.SubmitChanges());


            var result = TestedController.SaveDelete(postID);


            VerifyRedirectToRouteResult(result, expectedAction: "Index");
        }


        [Test]
        public void SaveDeleteNotFoundTest()
        {
            var postID = "TestPostID";


            _postRepositoryMock.Expect(r => r.Get(postID)).Return(null);


            try
            {
                var result = TestedController.SaveDelete(postID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        private static void VerifyPost(Post postToVerify, string expectedPostID, string expectedTitle, string expectedBody,
            bool expectedIsPublished, Position expectedPosition, PostCategory expectedCategory,
            string[] expectedTags, DateTime? expectedPublishDate,
            DateTime? expectedDateFrom, DateTime? expectedDateTo)
        {
            Assert.AreEqual(postToVerify.ID, expectedPostID, "Post ID has been modified.");
            Assert.AreEqual(postToVerify.Title, expectedTitle, "Title not set.");
            Assert.AreEqual(postToVerify.Body, expectedBody, "Body not set.");
            Assert.AreEqual(postToVerify.IsPublished, expectedIsPublished, "IsPublished not set.");
            Assert.AreEqual(postToVerify.Position, expectedPosition, "Position not set.");
            Assert.AreEqual(postToVerify.Category, expectedCategory, "Category not set.");
            Assert.That(postToVerify.Tags, Is.EqualTo(expectedTags), "Tags not set.");
            Assert.AreEqual(postToVerify.PublishDate, expectedPublishDate, "PublishDate not set.");
            Assert.AreEqual(postToVerify.DateFrom, expectedDateFrom, "DateFrom not set.");
            Assert.AreEqual(postToVerify.DateTo, expectedDateTo, "DateTo not set.");
        }
    }
}