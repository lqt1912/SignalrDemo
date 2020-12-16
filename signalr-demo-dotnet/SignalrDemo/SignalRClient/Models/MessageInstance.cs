using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SignalRClient.Models
{
    public class MessageInstance
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}