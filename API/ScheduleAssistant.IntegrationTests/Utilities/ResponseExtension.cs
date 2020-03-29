using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScheduleAssistant.IntegrationTests.Utilities
{
    public static class ResponseExtension
    {
        public static async Task<T> DeserializeContentAsync<T>(this HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
