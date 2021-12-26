using System.Net.Http;

namespace Application.Commons.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Convert content from HttpContent to string
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public static string ContentToString(this HttpContent httpContent)
        {
            var readAsStringAsync = httpContent.ReadAsStringAsync();
            return readAsStringAsync.Result;
        }
    }
}