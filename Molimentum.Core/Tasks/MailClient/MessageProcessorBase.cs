using System;
using System.Globalization;
using Molimentum.Model;

namespace Molimentum.Tasks.MailClient
{
    public abstract class MessageProcessorBase : IMessageProcessor
    {
        protected MessageProcessorBase(string supportedMessageCategory)
        {
            SupportedMessageCategory = supportedMessageCategory;
        }

        public string SupportedMessageCategory { get; private set; }

        public virtual void ProcessMessage(ParsedMessage parsedMessage)
        {
            if (!SupportedMessageCategory.Equals(parsedMessage.Category, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException(String.Format("Message is not of category '{0}'.", SupportedMessageCategory), "parsedMessage");
        }

        protected static void SetTags(ParsedMessage parsedMessage, ITaggable taggable)
        {
            if (parsedMessage["TAGS"] == null) return;

            taggable.Tags.Clear();

            foreach (var tag in parsedMessage["TAGS"].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) taggable.Tags.Add(tag);
        }

        protected static void SetPosition(ParsedMessage parsedMessage, IEditablePosition editablePosition)
        {
            var position = ParsePosition(parsedMessage);
            var positionDateTime = ParseDateTime(parsedMessage);

            editablePosition.Position = position;

            if (positionDateTime != null) editablePosition.PositionDateTime = positionDateTime;
        }

        protected static DateTime? ParseDateTime(ParsedMessage parsedMessage)
        {
            return ParseDateTime(parsedMessage, "TIME");
        }

        protected static DateTime? ParseDateTime(ParsedMessage parsedMessage, string field)
        {
            DateTimeOffset positionDateTime;

            if (!DateTimeOffset.TryParse(parsedMessage[field], new CultureInfo("DE-AT", false).DateTimeFormat, DateTimeStyles.AssumeUniversal, out positionDateTime)) return null;

            return positionDateTime.DateTime;
        }

        protected static Position ParsePosition(ParsedMessage parsedMessage)
        {
            if (parsedMessage["LATITUDE"] == null || parsedMessage["LONGITUDE"] == null) return null;

            Position position;

            return Position.TryParse(parsedMessage["LATITUDE"], parsedMessage["LONGITUDE"], out position) ? position : (Position)null;
        }
    }
}