using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DarkWebChat.RestServices
{
    public class DarkWebChatHub : Hub
    {
        public void BroadCastMessage(string msgFrom, string msg)
        {
            this.Clients.All.receiveMessage(msgFrom, msg);
        }
    }
}