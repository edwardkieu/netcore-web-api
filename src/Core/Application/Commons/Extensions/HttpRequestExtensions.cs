using System;

namespace Application.Commons.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetUri(this Microsoft.AspNetCore.Http.HttpRequest request, bool addPath = true, bool addQuery = true)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = (addPath ? request.Path.ToString() : default(string)) ?? throw new InvalidOperationException(),
                Query = (addQuery ? request.QueryString.ToString() : default(string)) ?? throw new InvalidOperationException()
            };
            return uriBuilder.Uri;
        }

        public static string HostWithNoSlash(this Uri uri)
        {
            return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
        }
    }
}