namespace DarkWebChat.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    using SignalRChat.Common;

    [HubName("DarkWebChatHub")]
    public class DarkWebChatHub : Hub
    {
        private static readonly Dictionary<string, List<UserDetail>> channelsOnlineUsers =
            new Dictionary<string, List<UserDetail>>();

        private static readonly List<UserDetail> chatOnlineUsers = new List<UserDetail>(); 

        public void ConnectUser(string userName)
        {
            var user = chatOnlineUsers.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                chatOnlineUsers.Add(
                new UserDetail { ConnectionId = this.Context.ConnectionId, UserName = userName });
            }
            else
            {
                user.ConnectionId = this.Context.ConnectionId;
            }
        }

        public Task JoinChannel(string userName, string channelName)
        {
            if (!channelsOnlineUsers.ContainsKey(channelName))
            {
                channelsOnlineUsers[channelName] = new List<UserDetail>();
            }

            // send to caller
            this.Clients.Caller.onConnected(channelsOnlineUsers[channelName].OrderBy(u => u.UserName));

            channelsOnlineUsers[channelName].Add(
                new UserDetail { ConnectionId = this.Context.ConnectionId, UserName = userName });

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
            this.Clients.Group(channelName).onChannelMessageReceived(message);
        }

        public void SendPrivateMessage(string username, string message)
        {
            var fromUserConnetionId = this.Context.ConnectionId;
            var toUserConnectionId = chatOnlineUsers.FirstOrDefault(u => u.UserName == username).ConnectionId;

            // send to 
            this.Clients.Client(toUserConnectionId).onPrivateMessageRecieved(fromUserConnetionId, message);

            // send to caller user
            this.Clients.Caller.onSentPrivateMessage(toUserConnectionId, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = this.Context.ConnectionId;

            foreach (var channel in channelsOnlineUsers.Keys)
            {
                var user = chatOnlineUsers.FirstOrDefault(u => u.ConnectionId == connectionId);
                var userInChannel = channelsOnlineUsers[channel].FirstOrDefault(u => u.ConnectionId == connectionId);

                if (userInChannel != null)
                {
                    channelsOnlineUsers[channel].Remove(userInChannel);
                    this.Clients.All.onUserDisconnected(userInChannel.UserName);
                }

                if (user != null)
                {
                    chatOnlineUsers.Remove(user);
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}