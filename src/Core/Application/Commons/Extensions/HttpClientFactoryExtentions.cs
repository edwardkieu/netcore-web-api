using Application.DTOs.HttpClient;
using Application.Enums;
using Application.Wrappers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Extensions
{
    public static class HttpClientFactoryExtentions
    {
        /// <summary>
        /// REGISTER: services.AddHttpClient(), Constants.ProductAPIBase = Configuration["ServiceUrls:ProductAPI"];
        /// USE INJECT HTTP CLIENT FACTORY INTO SERVICE AND CALL: https://github.com/edwardkieu/netcore-microservices/blob/master/WebMVC/Services/CartService.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T>(this IHttpClientFactory httpClient, ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("Name_API");
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }

                message.Method = apiRequest.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
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