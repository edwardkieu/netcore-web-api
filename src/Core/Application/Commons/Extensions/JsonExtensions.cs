using Newtonsoft.Json;

namespace Application.Commons.Extensions
{
    public static class JsonExtensions
    {
        //var newItem = JsonExtensions.Clone(oldItem);
        /// <summary>
        /// Clone an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            var serialized = System.Text.Json.JsonSerializer.Serialize(source);
            return System.Text.Json.JsonSerializer.Deserialize<T>(serialized);
        }

        /// <summary>
        /// Deserialize Json to Object without exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
    }
}