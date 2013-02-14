namespace Molimentum.Tasks.MailClient
{
    public interface IMessageProcessor
    {
        string SupportedMessageCategory { get; }
        void ProcessMessage(ParsedMessage parsedMessage);
    }
}
