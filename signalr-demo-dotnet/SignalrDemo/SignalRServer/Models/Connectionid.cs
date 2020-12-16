using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class Connectionid
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ConnectionId { get; set; }
    }
}
