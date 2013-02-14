using System.IO;
using System.Linq;
using Molimentum.Repositories;
using Molimentum.Tasks.MailClient.Configuration;

namespace Molimentum.Tasks.MailClient
{
    public class PictureMessageProcessor : MessageProcessorBase
    {
        private readonly IPictureRepository _pictureRepository;

        public PictureMessageProcessor(IPictureRepository pictureRepository)
            : base("PICTURE")
        {
            _pictureRepository = pictureRepository;
        }

        public override void ProcessMessage(ParsedMessage parsedMessage)
        {
            base.ProcessMessage(parsedMessage);

            var position = ParsePosition(parsedMessage);
            var positionDateTime = ParseDateTime(parsedMessage);

            using (var imageStream = new MemoryStream(parsedMessage.Attachments.First().Data))
            {
                imageStream.Seek(0, SeekOrigin.Begin);

                _pictureRepository.AddPictureToAlbum(
                    MailClientConfiguration.Settings.Pictures.AlbumID,
                    imageStream,
                    parsedMessage.Attachments.First().Name,
                    parsedMessage["TITLE"],
                    parsedMessage.Body,
                    positionDateTime,
                    position
                );
            }
        }
    }
}