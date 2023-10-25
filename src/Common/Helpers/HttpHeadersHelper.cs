using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Common.Helpers
{
    public class HttpHeadersHelper
    {
        public static List<string> ExtractHeaders(IHeaderDictionary headers, string[]? headerKeys)
        {
            List<string> headerValues = new();

            if (headerKeys is not null && headerKeys.Length > 0)
            {
                foreach (var headerKey in headerKeys)
                {
                    StringValues headerValue = headers.FirstOrDefault(header => header.Key == headerKey).Value;

                    string result = $"{headerKey}:{headerValue}";

                    headerValues.Add(result);
                }
            }

            return headerValues;
        }
    }
}
