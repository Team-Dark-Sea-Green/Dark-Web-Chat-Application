namespace DarkWebChat.Web
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.SignalR;

    using Microsoft.AspNet.SignalR.Hubs;

    using SignalRChat.Common;

    [HubName("DarkWebChatHub")]
    public class DarkWebChatHub : Hub
    {
        private static Dictionary<string, List<UserDetail>> channelsOnlineUsers = new Dictionary<string, List<UserDetail>>();

        public Task JoinChannel(string userName, string channelName)
        {
            if (!channelsOnlineUsers.ContainsKey(channelName))
            {
                channelsOnlineUsers[channelName] = new List<UserDetail>(); 
            }

            // send to caller
            this.Clients.Caller.onConnected(channelsOnlineUsers[channelName]);

            channelsOnlineUsers[channelName].Add(
               new UserDetail() { ConnectionId = this.Context.ConnectionId, UserName = userName });

            var id = this.Context.ConnectionId;
            
            // send to all in group except caller client
            this.Clients.OthersInGroup(channelName).onNewUserConnected(id, userName);
            return this.Groups.Add(this.Context.ConnectionId, channelName);
        }

        public Task LeaveChannel(string channelName)
        {
            var connectionId = this.Context.ConnectionId;
            var user = channelsOnlineUsers[channelName].FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user != null)
            {
                channelsOnlineUsers[channelName].Remove(user);

                this.Clients.All.onUserDisconnected(user.UserName);
            }

            return this.Groups.Remove(connectionId, channelName);
        }

        public void SendMessageToGroup(string message, string channelName)
        {
            // Broad cast message
            this.Clients.Group(channelName).messageReceived(message);
        }

        public void SendPrivateMessage(string toUserConnetionId, string message, string channelName)
        {
            string fromUserConnetionId = this.Context.ConnectionId;

            var toUser = channelsOnlineUsers[channelName].FirstOrDefault(x => x.ConnectionId == toUserConnetionId);
            var fromUser = channelsOnlineUsers[channelName].FirstOrDefault(x => x.ConnectionId == fromUserConnetionId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                this.Clients.Client(toUserConnetionId).onPrivateMessageRecieved(fromUserConnetionId, fromUser.UserName, message);

                // send to caller user
                this.Clients.Caller.onSentPrivateMessage(toUserConnetionId, fromUser.UserName, message);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = this.Context.ConnectionId;

            foreach (var channel in channelsOnlineUsers.Keys)
            {
                var user = channelsOnlineUsers[channel].FirstOrDefault(u => u.ConnectionId == connectionId);
                if (user != null)
                {
                    channelsOnlineUsers[channel].Remove(user);

                    this.Clients.All.onUserDisconnected(user.UserName);
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}