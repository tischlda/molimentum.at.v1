using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Molimentum.Tasks.MailClient
{
    public class ParsedMessage : IEnumerable<KeyValuePair<string, string>>
    {
        public ParsedMessage(string from, string to, string subject, string body, IEnumerable<MailAttachment> attachments)
        {
            From = from;
            To = to;
            Category = subject;
            Attachments = attachments;

            using(var lines = new StringReader(body))
            {
                var inHeader = true;
                string line;
                var bodyBuilder = new StringBuilder();

                while((line = lines.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        inHeader = false;
                    }
                    else
                    {
                        if (inHeader)
                        {
                            var parts = line.Split(new[] {':'}, 2);
                            if (parts.Length != 2)
                            {
                                inHeader = false;
                            }
                            else
                            {
                                _arguments[parts[0].Trim().ToUpper()] = parts[1].Trim();
                            }
                        }
                        if (!inHeader)
                        {
                            bodyBuilder.AppendLine(line);
                        }
                    }
                }

                Body = bodyBuilder.ToString().Trim();
            }
        }

        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return _arguments.ContainsKey(key) ? _arguments[key] : null;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Category { get; private set; }

        public string Body { get; private set; }

        public string To { get; private set; }

        public string From { get; private set; }

        public IEnumerable<MailAttachment> Attachments { get; private set; }
    }
}