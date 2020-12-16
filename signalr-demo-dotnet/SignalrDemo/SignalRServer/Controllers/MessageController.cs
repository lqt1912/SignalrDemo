using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MsgPack;
using SignalrDemo.Models;
using SignalrDemo.Signalr;
using SignalRServer.Models;

namespace SignalRServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<SignalRHub<MessageInstance>, IHubClient<MessageInstance>> _signalrHub;
        IHttpContextAccessor _contextAccessor;
        IConfiguration configuration;

        public MessageController(IHubContext<SignalRHub<MessageInstance>, IHubClient<MessageInstance>> signalrHub, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _signalrHub = signalrHub;
            _contextAccessor = contextAccessor;
            this.configuration = configuration;
        }

        [HttpPost("client/add")]
        public IActionResult AddToGroup(string connectionId, string groupName)
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<GroupMessage>("group");

            var currentGroup = msgCollection.AsQueryable().SingleOrDefault(x => x.GroupName == groupName);

            currentGroup.ConnectionIds.Add(connectionId);

            var filter = Builders<GroupMessage>.Filter.Eq("_id", currentGroup.Id);
            msgCollection.ReplaceOne(filter, currentGroup);
            return Ok($"Add {connectionId} to {groupName}");
        }

        [HttpPost("group/add")]
        public IActionResult Add(string groupName)
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<GroupMessage>("group");
            var group = new GroupMessage();
            group.GroupName = groupName;
            msgCollection.InsertOne(group);

            return Ok(group.GroupName);
        }
        [HttpPost("send")]
        public async Task<IActionResult> Post([FromBody] MessageInstance msg, string groupName)
        {
            var retMessage = string.Empty;
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<MessageInstance>("message");

            var scheme = _contextAccessor.HttpContext.Request.Scheme;
            var host = _contextAccessor.HttpContext.Request.Host;
            var pathBase = _contextAccessor.HttpContext.Request.PathBase;
            var location = $"{scheme}://{host}{pathBase}";

            var groups = database.GetCollection<GroupMessage>("group");

            var a = groups.AsQueryable().SingleOrDefault(x => x.GroupName == groupName);
            var connectionIds = new List<string>();
            if(a!=null)
               connectionIds =  a.ConnectionIds;


            try
            {
                msg.Timestamp = DateTime.Now.ToString();
                msg.From = location;

                if (!String.IsNullOrEmpty(groupName))
                {
                    await _signalrHub.Clients.Clients(connectionIds).SendNofti(msg);
                }
                else await _signalrHub.Clients.All.SendNofti(msg);

                await msgCollection.InsertOneAsync(msg);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return Ok(msg);
        }

        [HttpGet("all")]
        public IActionResult GetAllMessage()
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<MessageInstance>("message");
            return Ok(msgCollection.AsQueryable().ToList());
        }

        [HttpDelete("delete")]
        public IActionResult Delete()
        {
            MongoClient client = new MongoClient(configuration["MongoConnectionString"]);
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<MessageInstance>("message");
            foreach (var item in msgCollection.AsQueryable())
            {
                var deleteFilter = Builders<MessageInstance>.Filter.Eq("_id", item.Id);
                msgCollection.DeleteOne(deleteFilter);
            }

            return Ok("Deleted");
        }



    }
}
