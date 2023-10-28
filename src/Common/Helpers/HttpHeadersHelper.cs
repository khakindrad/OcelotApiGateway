using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;

namespace Common.Helpers;

public static class HttpHeadersHelper
{
    public static ReadOnlyCollection<string> ExtractHeaders(IHeaderDictionary headers, string[]? headerKeys)
    {
        List<string> headerValues = new();

        if (headerKeys is not null && headerKeys.Length > 0)
        {
            foreach (var headerKey in headerKeys)
            {
                var headerValue = headers.FirstOrDefault(header => string.Equals(header.Key, headerKey, StringComparison.Ordinal)).Value;

                var result = $"{headerKey}:{headerValue}";

                headerValues.Add(result);
            }
        }

        return headerValues.AsReadOnly();
    }
}
