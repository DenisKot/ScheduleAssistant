using Newtonsoft.Json;

namespace ScheduleAssistant.Communication
{
    public class BaseEntityDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}