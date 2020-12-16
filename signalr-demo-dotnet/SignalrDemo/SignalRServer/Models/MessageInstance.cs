using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SignalrDemo.Models
{
    public class MessageInstance
    {
        public MessageInstance()
        {
            Id = ObjectId.GenerateNewId();
        }
        [BsonId]
        public ObjectId Id { get; set; }

        public string Timestamp { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public string ConnectionId { get; set; }
    }
}