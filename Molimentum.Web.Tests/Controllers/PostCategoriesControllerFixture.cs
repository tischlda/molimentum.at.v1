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
    public class PostCategoriesControllerFixture : ControllerFixtureBase<PostCategoriesController>
    {
        private IPostCategoryRepository _postCategoryRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postCategoryRepositoryMock = MockRepository.StrictMock<IPostCategoryRepository>();
        }

        
        protected override PostCategoriesController CreateTestedController()
        {
            return new PostCategoriesController(_postCategoryRepositoryMock);
        }



        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;

            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(10));
        }


        [Test]
        public void DetailTest()
        {
            var postCategoryID = "TestPostCategoryID";

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;


            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(postCategory);


            var result = TestedController.Detail(postCategoryID);


            VerifyViewResult(result, "Detail", typeof(PostCategory), postCategory);
        }

        
        [Test]
        public void DetailByTitleTest()
        {
            var title = "Title";

            var postCategoryID = "TestPostCategoryID";

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;


            _postCategoryRepositoryMock.Expect(r => r.GetByTitle(title)).Return(postCategory);


            var result = TestedController.DetailByTitle(title);


            VerifyViewResult(result, "Detail", typeof(PostCategory), postCategory);
        }

        
        [Test]
        public void DetailNotFoundTest()
        {
            var postCategoryID = "TestPostCategoryID";


            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(null);


            try
            {
                var result = TestedController.Detail(postCategoryID);

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
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage)
        {
            var expectedPageSize = TestedController.DefaultPageSize;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var postCategoryListPage = new PagedList<PostCategory>(new PostCategory[] { }, expectedPage, expectedPageSize, pages);

            _postCategoryRepositoryMock.Expect(r => r.List(expectedRequestedPage, expectedPageSize)).Return(postCategoryListPage);


            var result = TestedController.Index(requestedPage);


            VerifyViewResult(result, "List", typeof(PagedList<PostCategory>), postCategoryListPage);
        }


        [Test]
        public void AddTest()
        {
            var postCategoryID = Guid.Empty.ToString();

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;

            AddRole("Author");
            _postCategoryRepositoryMock.Expect(r => r.Create()).Return(postCategory);


            var result = TestedController.Add();


            VerifyViewResult(result, "Edit", typeof(EditPostCategoryData));
            var resultModel = (EditPostCategoryData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PostCategory, Is.EqualTo(postCategory));
        }


        [Test]
        public void EditTest()
        {
            var postCategoryID = "TestPostCategoryID";

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;

            AddRole("Author");
            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(postCategory);


            var result = TestedController.Edit(postCategoryID);


            VerifyViewResult(result, "Edit", typeof(EditPostCategoryData));
            var resultModel = (EditPostCategoryData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(resultModel.PostCategory, Is.EqualTo(postCategory));
        }


        [Test]
        public void EditNotFoundTest()
        {
            var postCategoryID = "TestPostCategoryID";

            AddRole("Author");
            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(null);
  

            try
            {
                var result = TestedController.Edit(postCategoryID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }


        [Test, Sequential]
        public void SaveNewTest(
            [Values(PostListOrder.Date, PostListOrder.Title)] PostListOrder postListOrder)
        {
            var editMode = EditMode.Add;

            var postCategoryID = Guid.Empty.ToString();
            var title = "A post category title";
            var body = string.Empty;

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;

            _postCategoryRepositoryMock.Expect(r => r.Create()).Return(postCategory);
            _postCategoryRepositoryMock.Expect(r => r.Save(postCategory));
            _postCategoryRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPostCategory(postCategory, postCategoryID, title, body, postListOrder));


            var result = TestedController.Save(editMode, postCategoryID, title, body, postListOrder);


            VerifyRedirectToRouteResult(result, expectedAction: "Detail", expectedID: postCategoryID);
        }


        [Test, Sequential]
        public void SaveExistingTest(
            [Values(PostListOrder.Date, PostListOrder.Title)] PostListOrder postListOrder)
        {
            var editMode = EditMode.Edit;

            var postCategoryID = "TestPostCategoryID";
            var title = "A post category title";
            var body = string.Empty;

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;
            postCategory.Title = "oldTitle";
            postCategory.Body = "oldBody";
            postCategory.PostListOrder = (PostListOrder)Int32.MaxValue;

            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(postCategory);
            _postCategoryRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPostCategory(postCategory, postCategoryID, title, body, postListOrder));


            var result = TestedController.Save(editMode, postCategoryID, title, body, postListOrder);


            VerifyRedirectToRouteResult(result, expectedAction: "Detail", expectedID: postCategoryID);
        }


        [Test]
        public void SaveInvalidTest(
            [Values("", null)] string title)
        {
            var editMode = EditMode.Add;

            var postCategoryID = Guid.Empty.ToString();
            var body = string.Empty;
            var postListOrder = PostListOrder.Title;

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;

            _postCategoryRepositoryMock.Expect(r => r.Create()).Return(postCategory);


            var result = TestedController.Save(editMode, postCategoryID, title, body, postListOrder);


            VerifyViewResult(result, "Edit", typeof(EditPostCategoryData));
            var resultModel = (EditPostCategoryData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PostCategory, Is.EqualTo(postCategory));
        }


        [Test]
        public void SaveWithInvalidEditModeTest()
        {
            var editMode = (EditMode)Int32.MaxValue;

            var postCategoryID = Guid.Empty.ToString();
            var title = "A post category title";
            var body = string.Empty;
            var postListOrder = PostListOrder.Title;

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;


            try
            {
                var result = TestedController.Save(editMode, postCategoryID, title, body, postListOrder);
                
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
            var postCategoryID = "TestPostCategoryID";

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;


            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(postCategory);


            var result = TestedController.Delete(postCategoryID);


            VerifyViewResult(result, "ConfirmDelete", typeof(PostCategory), postCategory);
        }


        [Test]
        public void SaveDeleteTest()
        {
            var postCategoryID = "TestPostCategoryID";

            var postCategory = new PostCategory();
            postCategory.ID = postCategoryID;


            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(postCategory);
            _postCategoryRepositoryMock.Expect(r => r.Delete(postCategory));
            _postCategoryRepositoryMock.Expect(r => r.SubmitChanges());


            var result = TestedController.SaveDelete(postCategoryID);


            VerifyRedirectToRouteResult(result, expectedAction: "Index");
        }


        [Test]
        public void SaveDeleteNotFoundTest()
        {
            var postCategoryID = "TestPostCategoryID";


            _postCategoryRepositoryMock.Expect(r => r.Get(postCategoryID)).Return(null);


            try
            {
                var result = TestedController.SaveDelete(postCategoryID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }


        private static void VerifyPostCategory(PostCategory postCategoryToVerify, string expectedPostCategoryID, string expectedTitle,
           string expectedBody, PostListOrder expectedPostListOrder)
        {
            Assert.AreEqual(postCategoryToVerify.ID, expectedPostCategoryID, "PostCategory ID has been modified.");
            Assert.AreEqual(postCategoryToVerify.Title, expectedTitle, "Title not set.");
            Assert.AreEqual(postCategoryToVerify.Body, expectedBody, "Body not set.");
            Assert.AreEqual(postCategoryToVerify.PostListOrder, expectedPostListOrder, "PostListOrder not set.");
        }
    }
}