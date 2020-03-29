using System;
using Newtonsoft.Json;

namespace ScheduleAssistant.Communication.Windows
{
    public class WindowDto : BaseEntityDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("finish")]
        public DateTimeOffset Finish { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("type")]
        public WindowTypeDto Type { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }
    }
}
