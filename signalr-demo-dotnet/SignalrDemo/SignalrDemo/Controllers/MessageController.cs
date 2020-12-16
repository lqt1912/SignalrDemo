using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using MsgPack;
using SignalrDemo.Models;
using SignalrDemo.Signalr;

namespace SignalrDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<SignalrHub, IHubClient> _signalrHub;

        public MessageController(IHubContext<SignalrHub, IHubClient> signalrHub)
        {
            _signalrHub = signalrHub;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Post([FromBody] MessageInstance msg)
        {
            var retMessage = string.Empty;
            MongoClient client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<MessageInstance>("message");
            try
            {
                msg.Timestamp = Timestamp.UtcNow.ToString();
                await _signalrHub.Clients.All.BroadcastMessage(msg);
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
            MongoClient client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            IMongoDatabase database = client.GetDatabase("signalrmessage");
            var msgCollection = database.GetCollection<MessageInstance>("message");
            return Ok(msgCollection.AsQueryable().ToList());
        }
    }
}
