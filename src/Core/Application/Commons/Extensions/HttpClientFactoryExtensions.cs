using Application.Commons.Helpers;
using Application.DTOs.HttpClient;
using Application.Wrappers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        /// <summary>
        /// REGISTER: services.AddHttpClient(), Constants.ProductAPIBase = Configuration["ServiceUrls:ProductAPI"];
        /// USE INJECT HTTP CLIENT FACTORY INTO SERVICE AND CALL: https://github.com/edwardkieu/netcore-microservices/blob/master/WebMVC/Services/CartService.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="baseApiRequest"></param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T>(this IHttpClientFactory httpClient, BaseApiRequest baseApiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("Name_API");
                var message = new HttpRequestMessage();
                if (baseApiRequest is ApiRequestWidthFile apiRequestWithFile)
                {
                    using var formData = new MultipartFormDataContent();
                    using var streamContent = new StreamContent(apiRequestWithFile.FormFile.OpenReadStream());
                    using var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    apiRequestWithFile.Data.ToDictionary().Select(x => x).ForAll(x =>
                    {
                        formData.Add(new StringContent(x.Value.ToString()), x.Key);
                    });

                    formData.Add(fileContent, "file", apiRequestWithFile.FormFile.FileName);
                    message.Content = formData;
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                    message.RequestUri = new Uri(baseApiRequest.Url);
                    client.DefaultRequestHeaders.Clear();
                    if (baseApiRequest.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(baseApiRequest.Data), Encoding.UTF8, "application/json");
                    }

                    if (!string.IsNullOrEmpty(baseApiRequest.AccessToken))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", baseApiRequest.AccessToken);
                    }
                }

                message.Method = baseApiRequest.Method;
                var apiResponse = await client.SendAsync(message).ConfigureAwait(false);
                var apiContent = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var apiResponse = new Response<string>($"Error: {Convert.ToString(ex.Message)}");
                var res = JsonConvert.SerializeObject(apiResponse);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }
    }
}