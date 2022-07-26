using Newtonsoft.Json;

namespace BankServer.Test
{
    public static class HttpResponseMessageExtensions
    {
        public static T? ConvertTo<T>(this HttpResponseMessage response)
        {
            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<T?> ConvertTo<T>(this Task<HttpResponseMessage> response)
        {
            var json = await (await response).Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
