namespace Molimentum.Tasks.MailClient
{
    public class MailAttachment
    {
        public MailAttachment(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; private set; }
        public byte[] Data { get; private set; }
    }
}