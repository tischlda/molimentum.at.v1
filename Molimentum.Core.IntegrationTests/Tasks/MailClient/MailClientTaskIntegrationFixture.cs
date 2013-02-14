using Molimentum.Providers.Google;
using NUnit.Framework;
using Molimentum.Providers.NHibernate;

namespace Molimentum.Tasks.MailClient.IntegrationTests
{
    /// <summary>
    /// Processes mails at the configured location.
    /// </summary>
    [TestFixture]
    //[Explicit("Integration")]
    [Category("Integration")]
    public class MailClientTaskIntegrationFixture
    {
        [Test]
        public void MailTest()
        {
            var sessionBuilder = new NHibernateSessionBuilder();
            var pictureRepository = new PicasaPictureRepository();
            var postRepository = new NHibernatePostRepository(sessionBuilder);
            var postCategoryRepository = new NHibernatePostCategoryRepository(sessionBuilder);
            var pictureMessageProcessor = new PictureMessageProcessor(pictureRepository);
            var postMessageProcessor = new PostMessageProcessor(postRepository, postCategoryRepository, pictureRepository);
            var mailClient = new PopMailProvider.PopMailService();
            var mailClientTask = new MailClientTask(mailClient, new IMessageProcessor[] {postMessageProcessor, pictureMessageProcessor});
            mailClientTask.Execute();

            Assert.Inconclusive("Manual check required.");
        }
    }
}
