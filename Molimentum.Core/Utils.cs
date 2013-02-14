using System;
using System.Collections;
using System.Text;

namespace Molimentum
{
    public static class Utils
    {
        public static int CalculatePages(int pageSize, long itemCount)
        {
            if (pageSize <= 0) throw new ArgumentException("The pageSize must be larger than 0.", "pageSize");
            if (itemCount < 0) throw new ArgumentException("The itemCount must be larger than or equal to 0.", "itemCount");

            return Math.Max(1, (int)Math.Ceiling((double)itemCount / pageSize));
        }

        public static int CalculatePageNumber(int itemIndex, int pageSize)
        {
            return (int)Math.Floor((double)itemIndex / (double)pageSize) + 1;
        }

        public static string Concat(this IEnumerable items, string delimiter)
        {
            var result = new StringBuilder();

            foreach (var item in items)
            {
                if (result.Length > 0) result.Append(delimiter);
                result.Append(item);
            }

            return result.ToString();
        }

        public static string QuoteBody(string body)
        {
            var quotedBody = new StringBuilder();

            foreach (var line in body.Split(new[] { '\n' }))
            {
                quotedBody.Append("> ");
                quotedBody.Append(line);
            }

            quotedBody.AppendLine();
            quotedBody.AppendLine();

            return quotedBody.ToString();
        }
    }
}
