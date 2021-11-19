using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ModelViews;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public class MessageHub : Hub, ImessageHub
    {

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            var y = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var identity = (ClaimsIdentity)Context.User.Identity;

            var tmp = identity.FindFirst(ClaimTypes.NameIdentifier);

            var z = Context.User.Claims.ToArray()[2].Value;

          

            return base.OnConnectedAsync();
        }
        public async Task Newid(string id)
        {
            string name = id;
            await _context.Groups.AddToGroupAsync(Context.ConnectionId, name);
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        public async Task NewMsgNotify(string userID , string username , string msg)
        {
            
            await _context.Clients.User(userID).SendAsync("newMsg", "You got new Msg from " +username +": \n"+msg);
         
        }
        public async Task NewMessage(Message msg)
        {
           
            var x = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await Clients.Clients(Context.User.Claims.ToArray()[2].Value).SendAsync("MessageReceived", msg);
          
        }
        protected IHubContext<MessageHub> _context;

        public MessageHub(IHubContext<MessageHub> context)
        {
            this._context = context;
        }


      
    }
}
