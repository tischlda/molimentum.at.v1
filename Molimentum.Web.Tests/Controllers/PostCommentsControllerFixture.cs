using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using Molimentum.Web.Models;
using NUnit.Framework;
using Rhino.Mocks;
using Molimentum.Services;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class PostCommentsControllerFixture : ControllerFixtureBase<PostCommentsController>
    {
        private IPostCommentRepository _postCommentRepositoryMock;
        private IPostRepository _postRepositoryMock;
        private INotificationService _notificationServiceMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postCommentRepositoryMock = MockRepository.StrictMock<IPostCommentRepository>();
            _postRepositoryMock = MockRepository.StrictMock<IPostRepository>();
            _notificationServiceMock = MockRepository.StrictMock<INotificationService>();
        }


        protected override PostCommentsController CreateTestedController()
        {
            return new PostCommentsController(_postRepositoryMock, _postCommentRepositoryMock, _notificationServiceMock);
        }


        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;

            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(10));
        }


        [Test, Sequential]
        public void IndexTest(
            [Values(10, 10, 10, 10, 10)] int pages,
            [Values(null, 10, 12, null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage,
            [Values(null, 10, 20, null, 10, 20)] int? requestedPageSize,
            [Values("text", null, null, null, null, null)] string mode,
            [Values("ListText", "List", "List", "List", "List", "List")] string expectedView)
        {
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;
            var expectedPageSize = requestedPageSize == null ? TestedController.DefaultPageSize : requestedPageSize.Value;

            var postCommentListPage = new PagedList<PostComment>(new PostComment[] { }, expectedPage, expectedPageSize, pages);

            _postCommentRepositoryMock.Expect(r => r.ListPublished(expectedRequestedPage, expectedPageSize)).Return(postCommentListPage);


            var result = TestedController.Index(requestedPage, requestedPageSize, mode);


            VerifyViewResult(result, expectedView, typeof(PagedList<PostComment>), postCommentListPage);
        }


        [Test, Sequential]
        public void AddTest(
            [Values(null, "PostID")] string postID)
        {
            var postCommentID = Guid.Empty.ToString();

            var postComment = new PostComment();
            postComment.ID = postCommentID;

            _postCommentRepositoryMock.Expect(r => r.Create()).Return(postComment);

            Post post;

            if (postID != null)
            {
                post = new Post();
                post.ID = postID;

                _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            }
            else
            {
                post = null;
            }


            var result = TestedController.Add(postID);


            VerifyViewResult(result, "Edit", typeof(EditPostCommentData));
            var resultModel = (EditPostCommentData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PostComment.Post, Is.EqualTo(post));
        }


        [Test]
        public void AddWithPostNotFoundTest()
        {
            var postID = "TestPostID";

            _postRepositoryMock.Expect(r => r.Get(postID)).Return(null);


            try
            {
                var result = TestedController.Add(postID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }


        [Test]
        public void EditTest()
        {
            var postCommentID = "TestPostCommentID";

            var postComment = new PostComment();
            postComment.ID = postCommentID;

            _postCommentRepositoryMock.Expect(r => r.Get(postCommentID)).Return(postComment);


            var result = TestedController.Edit(postCommentID);


            VerifyViewResult(result, "Edit", typeof(EditPostCommentData));
            var resultModel = (EditPostCommentData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Edit));
        }


        [Test]
        public void EditNotFoundTest()
        {
            var postCommentID = "TestPostCommentID";


            _postCommentRepositoryMock.Expect(r => r.Get(postCommentID)).Return(null);


            try
            {
                var result = TestedController.Edit(postCommentID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }


        [Test]
        public void ReplyTest(
            [Values(null, "TestPostID")] string postID)
        {
            Post post;

            if (postID != null)
            {
                post = new Post();
                post.ID = postID;
            }
            else
            {
                post = null;
            }

            var originalPostCommentID = "TestPostCommentID";

            var originalPostComment = new PostComment();
            originalPostComment.Post = post;
            originalPostComment.ID = originalPostCommentID;
            originalPostComment.Title = "Original title";
            originalPostComment.Body = "Original body";


            var postCommentID = Guid.Empty.ToString();

            var postComment = new PostComment();
            postComment.ID = postCommentID;

            _postCommentRepositoryMock.Expect(r => r.Get(originalPostCommentID)).Return(originalPostComment);
            if (postID != null) _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            _postCommentRepositoryMock.Expect(r => r.Create()).Return(postComment);

            var result = TestedController.Reply(originalPostCommentID);


            VerifyViewResult(result, "Edit", typeof(EditPostCommentData));
            var resultModel = (EditPostCommentData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PostComment.Post, Is.EqualTo(originalPostComment.Post));
            Assert.That(resultModel.PostComment.Title, Is.EqualTo("Re: Original title"));
            Assert.That(resultModel.PostComment.Body, Is.EqualTo(Utils.QuoteBody(originalPostComment.Body)));
        }


        [Test]
        public void SaveNewTest(
            [Values(null, "PostID")] string postID)
        {
            var editMode = EditMode.Add;

            var postCommentID = Guid.Empty.ToString();
            var author = "Author<";
            var title = "A post comment title<";
            var body = "Post comment body<";
            var email = "me@here.com";
            var website = "http://www.here.com";
            var shipName = "Molimentum";

            
            Post post;

            if (postID != null)
            {
                post = new Post();
                post.ID = postID;

                _postRepositoryMock.Expect(r => r.Get(postID)).Return(post);
            }
            else
            {
                post = null;
            }

            
            var postComment = new PostComment();
            postComment.ID = postCommentID;
            

            _postCommentRepositoryMock.Expect(r => r.Create()).Return(postComment);
            _postCommentRepositoryMock.Expect(r => r.Save(postComment));
            _postCommentRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPostComment(postComment, postCommentID, postID, author, title, body, email, website));
            _notificationServiceMock.Expect(n => n.Notify("New Comment", postComment));


            TestedController.ValueProvider = new FormCollection
            {
                { "PostComment.Author", author },
                { "PostComment.Title", title },
                { "PostComment.Body", body },
                { "PostComment.Email", email },
                { "PostComment.Website", website }
            };

            var result = TestedController.Save(editMode, postCommentID, postID, shipName);


            if (postID != null)
            {
                // associated post, expect redirect to post detail page
                VerifyRedirectToRouteResult(result, expectedController:  "Posts", expectedAction: "Detail", expectedID: postID);
            }
            else
            {
                // no associated post, expect redirect to postcomments list
                VerifyRedirectToRouteResult(result, expectedAction: "Index");
            }
        }


        [Test]
        public void SaveExistingTest(
            [Values(null, "PostID")] string postID)
        {
            var editMode = EditMode.Edit;

            var postCommentID = "PostCommentID";
            var author = "Author<";
            var title = "A post comment title<";
            var body = "Post comment body<";
            var email = "me@here.com";
            var website = "http://www.here.com";
            var shipName = "Molimentum";


            Post post;

            if (postID != null)
            {
                post = new Post();
                post.ID = postID;
            }
            else
            {
                post = null;
            }


            var postComment = new PostComment();
            postComment.ID = postCommentID;
            postComment.Post = post;
            postComment.Author = "Old author";
            postComment.Title = "Old post comment title";
            postComment.Body = "Old post comment body";
            postComment.Email = "oldme@here.com";
            postComment.Website = "http://www.oldhere.com";


            _postCommentRepositoryMock.Expect(r => r.Get(postCommentID)).Return(postComment);
            _postCommentRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPostComment(postComment, postCommentID, postID, author, title, body, email, website));


            TestedController.ValueProvider = new FormCollection
            {
                { "PostComment.Author", author },
                { "PostComment.Title", title },
                { "PostComment.Body", body },
                { "PostComment.Email", email },
                { "PostComment.Website", website }
            };
            
            var result = TestedController.Save(editMode, postCommentID, postID, shipName);


            if (postID != null)
            {
                // associated post, expect redirect to post detail page
                VerifyRedirectToRouteResult(result, expectedController: "Posts", expectedAction: "Detail", expectedID: postID);
            }
            else
            {
                // no associated post, expect redirect to postcomments list
                VerifyRedirectToRouteResult(result, expectedAction: "Index");
            }
        }

        
        [Test]
        public void SaveWithInvalidEditModeTest()
        {
            var editMode = (EditMode)Int32.MaxValue;

            var postCommentID = Guid.Empty.ToString();
            var postID = "TestPostID";
            var author = "Author<";
            var title = "A post comment title<";
            var body = "Post comment body<";
            var email = "me@here.com";
            var website = "http://www.here.com";
            var shipName = "Molimentum";

            var postComment = new PostComment();
            postComment.ID = postCommentID;


            try
            {
                TestedController.ValueProvider = new FormCollection
                {
                    { "PostComment.Author", author },
                    { "PostComment.Title", title },
                    { "PostComment.Body", body },
                    { "PostComment.Email", email },
                    { "PostComment.Website", website }
                };
                
                var result = TestedController.Save(editMode, postCommentID, postID, shipName);

                Assert.Fail("ArgumentException not raised.");
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("editMode"));
            }
        }


        [Test, Sequential]
        [Ignore("Ignored temporary.")]
        public void SaveWithInvalidTest(
            [Values(true, false)] bool invalidAuthor,
            [Values(true, false)] bool invalidTitle,
            [Values(true, true, true, false)] bool invalidEmail,
            [Values(
                "",
                "me",
                "me@here.commmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
                "me@here.com")] string email,
            [Values(true, false)] bool invalidBody,
            [Values(true, false)] bool invalidWebsite)
        {
            var editMode = EditMode.Add;

            var postID = (string)null;

            var postCommentID = Guid.Empty.ToString();
            var author = invalidAuthor ? "" : "Author";
            var title = invalidTitle ? "" : "A post comment title";
            var body = invalidBody? "" : "Post comment body";
            var website = invalidWebsite ? "foobar.htt7holladrio\\\\" : "http://www.here.com";
            var shipName = "Blue Sky";

            var postComment = new PostComment();
            postComment.ID = postCommentID;


            _postCommentRepositoryMock.Expect(r => r.Create()).Return(postComment);


            TestedController.ValueProvider = new FormCollection
                {
                    { "PostComment.Author", author },
                    { "PostComment.Title", title },
                    { "PostComment.Body", body },
                    { "PostComment.Email", email },
                    { "PostComment.Website", website }
                };
            
            var result = TestedController.Save(editMode, postCommentID, postID, shipName);


            VerifyViewResult(result, "Edit", typeof(EditPostCommentData));

            var viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.False);
            Assert.That(viewResult.ViewData.ModelState.IsValidField("PostComment.Author"), Is.Not.EqualTo(invalidAuthor));
            Assert.That(viewResult.ViewData.ModelState.IsValidField("PostComment.Title"), Is.Not.EqualTo(invalidTitle));
            Assert.That(viewResult.ViewData.ModelState.IsValidField("PostComment.Body"), Is.Not.EqualTo(invalidBody));
            Assert.That(viewResult.ViewData.ModelState.IsValidField("PostComment.Email"), Is.Not.EqualTo(invalidEmail));
            Assert.That(viewResult.ViewData.ModelState.IsValidField("PostComment.Website"), Is.Not.EqualTo(invalidWebsite));
            Assert.That(viewResult.ViewData.ModelState.IsValidField("shipName"), Is.False);
            
            var resultModel = (EditPostCommentData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PostComment, Is.EqualTo(postComment));
            VerifyPostComment(resultModel.PostComment, postCommentID, postID, author, title, body, email, website);
        }

        
        [Test]
        public void DeleteTest()
        {
            var postCommentID = "TestPostCommentID";

            var postComment = new PostComment();
            postComment.ID = postCommentID;


            _postCommentRepositoryMock.Expect(r => r.Get(postCommentID)).Return(postComment);


            var result = TestedController.Delete(postCommentID);


            VerifyViewResult(result, "ConfirmDelete", typeof(PostComment), postComment);
        }


        [Test, Sequential]
        public void SaveDeleteTest(
            [Values(true, false)] bool hasAssociatedPost)
        {
            var postID = "TestPostID";
            var postCommentID = "TestPostCommentID";

            var postComment = new PostComment();
            postComment.ID = postCommentID;

            if (hasAssociatedPost)
            {
                var post = new Post();
                post.ID = postID;

                postComment.Post = post;
            }


            _postCommentRepositoryMock.Expect(r => r.Get(postCommentID)).Return(postComment);
            _postCommentRepositoryMock.Expect(r => r.Delete(postComment));
            _postCommentRepositoryMock.Expect(r => r.SubmitChanges());


            var result = TestedController.SaveDelete(postCommentID);

            if (hasAssociatedPost)
            {
                VerifyRedirectToRouteResult(result, expectedController: "Posts", expectedAction: "Detail", expectedID: postID);
            }
            else
            {
                VerifyRedirectToRouteResult(result, expectedAction: "Index");
            }
        }

        
        private void VerifyPostComment(PostComment postCommentToVerify, string postCommentID, string postID,
            string author, string title, string body, string email, string website)
        {
            Assert.AreEqual(postCommentToVerify.ID, postCommentID, "PostCommentID has been modified.");
            if (postID != null)
            {
                Assert.AreEqual(postCommentToVerify.Post.ID, postID, "Post has not been set.");
            }
            else
            {
                Assert.That(postCommentToVerify.Post, Is.Null, "Post is set.");
            }
            Assert.AreEqual(postCommentToVerify.Author, author, "Author not set.");
            Assert.AreEqual(postCommentToVerify.Title, title, "Title not set.");
            Assert.AreEqual(postCommentToVerify.Body, body, "Body not set.");
            Assert.AreEqual(postCommentToVerify.Email, email, "email not set.");
            Assert.AreEqual(postCommentToVerify.Website, website, "Website not set.");
        }
    }
}