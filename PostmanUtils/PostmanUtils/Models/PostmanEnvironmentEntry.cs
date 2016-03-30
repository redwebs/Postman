using Newtonsoft.Json;

namespace PostmanUtils.Models
{
    public class PostmanEnvironmentEntry
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "text";
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;
    }
}
