using Microsoft.AspNetCore.Http;

namespace Application.DTOs.HttpClient
{
    public class BaseApiRequest
    {
        public System.Net.Http.HttpMethod Method { get; set; } = System.Net.Http.HttpMethod.Get;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }

    public class ApiRequestWidthFile : BaseApiRequest
    {
        public IFormFile FormFile { get; set; }
    }
}