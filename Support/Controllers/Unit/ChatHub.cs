using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Support.Controllers.Unit
{
    public class ChatHub : Hub
    {
        public Task JoinLock(string lockNumber, long idChat)
        {
            string groupName = GetGroupName(lockNumber, idChat);
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public static string GetGroupName(string lockNumber, long idChat)
        {
            return "chat:" + lockNumber + ":" + idChat.ToString();
        }

        public static void NotifyNewMessage(string lockNumber, long idChat, object message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string groupName = GetGroupName(lockNumber, idChat);
            hubContext.Clients.Group(groupName).newMessage(message);
        }
    }

}