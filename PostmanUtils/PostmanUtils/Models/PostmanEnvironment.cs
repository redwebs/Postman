using System.Collections.Generic;
using Newtonsoft.Json;

namespace PostmanUtils.Models
{
    public class PostmanEnvironment
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("values")]
        public IEnumerable<PostmanEnvironmentEntry> Values { get; set; }
        [JsonProperty("team")]
        public string Team { get; set; }
        [JsonProperty("timeStamp")]
        public long TimeStamp { get; set; }
        [JsonProperty("synced")]
        public bool Synced { get; set; }
        [JsonProperty("syncedFilename")]
        public string SyncedFilename { get; set; } = "";
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
