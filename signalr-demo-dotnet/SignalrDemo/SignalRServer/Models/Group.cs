using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class GroupMessage
    {
        public GroupMessage()
        {
            Id = ObjectId.GenerateNewId();
            ConnectionIds = new List<string>();

        }
        [BsonId]
        public ObjectId Id { get; set; }

        public string GroupName { get; set; }
        public List<string> ConnectionIds { get; set; }
    }
}
