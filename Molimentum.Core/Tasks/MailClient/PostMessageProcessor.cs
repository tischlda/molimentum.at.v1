using System;
using System.IO;
using System.Text;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tasks.MailClient.Configuration;

namespace Molimentum.Tasks.MailClient
{
    public class PostMessageProcessor : MessageProcessorBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostCategoryRepository _postCategoryRepository;
        private readonly IPictureRepository _pictureRepository;

        private const string c_pictureTemplate = "<a href='{0}' rel='lightbox' title='{2}'><img src='{1}' alt='{2}' /></a>";


        public PostMessageProcessor(IPostRepository postRepository, IPostCategoryRepository postCategoryRepository, IPictureRepository pictureRepository)
            : base("POST")
        {
            _postRepository = postRepository;
            _postCategoryRepository = postCategoryRepository;
            _pictureRepository = pictureRepository;
        }

        public override void ProcessMessage(ParsedMessage parsedMessage)
        {
            base.ProcessMessage(parsedMessage);

            var post = GetPost(parsedMessage);

            post.Title = parsedMessage["TITLE"];

            post.Category = _postCategoryRepository.GetByTitle(parsedMessage["CATEGORY"]);

            post.PublishDate = ParseDateTime(parsedMessage);
            post.DateFrom = ParseDateTime(parsedMessage, "DATEFROM");
            post.DateTo = ParseDateTime(parsedMessage, "DATETO");
            post.IsPublished = true;

            SetPosition(parsedMessage, post);
            SetTags(parsedMessage, post);

            var bodyBuilder = new StringBuilder(parsedMessage.Body);

            //try
            //{
                var pictureCount = 0;

                if(parsedMessage.Attachments != null) foreach (var attachment in parsedMessage.Attachments)
                {
                    pictureCount++;

                    using (var imageStream = new MemoryStream(attachment.Data))
                    {
                        imageStream.Seek(0, SeekOrigin.Begin);

                        var picture = _pictureRepository.AddPictureToAlbum(
                            MailClientConfiguration.Settings.Pictures.AlbumID,
                            imageStream,
                            attachment.Name,
                            "",
                            "",
                            post.PositionDateTime,
                            post.Position);

                        bodyBuilder.Replace(String.Format("[PICTURE{0}]", pictureCount), CreatePictureHtml(picture));
                    }
                }
            //}
            //catch { ; }

            post.Body = bodyBuilder.ToString();

            if (parsedMessage["POSTID"] == null) _postRepository.Save(post);

            _postRepository.SubmitChanges();
        }

        private static string CreatePictureHtml(Picture picture)
        {
            return String.Format(c_pictureTemplate, picture.PictureUri.ToString().Replace("/s72/", "/s640/"), picture.ThumbnailUri.ToString().Replace("/s72/", "/s320/"), picture.Title);
        }

        private Post GetPost(ParsedMessage parsedMessage)
        {
            return parsedMessage["POSTID"] != null ? _postRepository.Get(parsedMessage["POSTID"]) : _postRepository.Create();
        }
    }
}