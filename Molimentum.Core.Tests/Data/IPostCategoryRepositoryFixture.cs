using System;
using System.Linq;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IPostCategoryRepositoryFixture
    {
        [Test]
        public void PostCategoryNotFoundTest()
        {
            using (var repository = CreateIPostCategoryRepository())
            {
                var postCategory = repository.Get(Guid.Empty.ToString());

                Assert.IsNull(postCategory);
            }
        }

        [Test]
        public void DeletePostCategoryTest()
        {
            var title = "Category";
            var body = "Body";

            using (var repository = CreateIPostCategoryRepository())
            {
                var postCategoryID = CreatePostCategory(repository, title, body);

                var postCategory = repository.Get(postCategoryID);
                repository.Delete(postCategory);
                repository.SubmitChanges();

                var postCategory2 = repository.Get(postCategoryID);

                Assert.IsNull(postCategory2);
            }
        }

        [Test]
        public void LoadAndSavePostCategoryTest()
        {
            var title = "Category";
            var body = "Body";

            using (var repository = CreateIPostCategoryRepository())
            {
                var postCategoryID = CreatePostCategory(repository, title, body);

                var postCategory = repository.Get(postCategoryID);

                Assert.AreEqual(title, postCategory.Title);
                Assert.AreEqual(body, postCategory.Body);
            }
        }

        [Test]
        public void LoadByTitleAndSavePostCategoryTest()
        {
            var title = "Category";
            var body = "Body";

            using (var repository = CreateIPostCategoryRepository())
            {
                var postCategoryID = CreatePostCategory(repository, title, body);

                var postCategory = repository.GetByTitle("Category");

                Assert.AreEqual(title, postCategory.Title);
                Assert.AreEqual(body, postCategory.Body);
            }
        }

        [Test]
        public void ListPostCategoriesTest()
        {
            var title = "Category";
            var body = "Body";

            using (var repository = CreateIPostCategoryRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePostCategory(repository, title + i, body);
                }

                var postCategories = repository.List(1, 20);

                Assert.That(postCategories, Is.Not.Null);
                Assert.That(postCategories.Items.Count(), Is.EqualTo(20));
            }
        }
        
        private static string CreatePostCategory(IPostCategoryRepository repository, string title, string body)
        {
            var postCategory = repository.Create();

            postCategory.Title = title;
            postCategory.Body = body;

            repository.Save(postCategory);

            repository.SubmitChanges();

            return postCategory.ID;
        }

        protected abstract IPostCategoryRepository CreateIPostCategoryRepository();
    }
}