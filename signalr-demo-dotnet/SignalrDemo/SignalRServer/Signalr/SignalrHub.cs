using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SignalrDemo.Models;
using SignalRServer.Models;

namespace SignalrDemo.Signalr
{

    public class SignalRHub<T> : Hub<IHubClient<T>> where T:class
    {
        IConfiguration configuration;

        public SignalRHub(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendNofti(T msg )
        {
            await Clients.All.SendNofti(msg);
        }

        public override  Task OnConnectedAsync()
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<Connectionid>("connection_id");

            var connectId = new Connectionid();
            connectId.ConnectionId = Context.ConnectionId;
             msgCollection.InsertOne(connectId);

            return   base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");

            var msgCollection = database.GetCollection<Connectionid>("connection_id");

                var deleteFilter = Builders<Connectionid>.Filter.Eq("ConnectionId", Context.ConnectionId);
                msgCollection.DeleteOne(deleteFilter);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddToGroup(string groupName)
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<GroupMessage>("group");

            var currentGroup = msgCollection.AsQueryable().SingleOrDefault(x=>x.GroupName == groupName);

            currentGroup.ConnectionIds.Add(Context.ConnectionId);

            var filter = Builders<GroupMessage>.Filter.Eq("_id", currentGroup.Id);
            msgCollection.ReplaceOne(filter, currentGroup);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }

}