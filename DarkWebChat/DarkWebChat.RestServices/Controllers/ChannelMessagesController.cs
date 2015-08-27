﻿namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;

    using Models.BindingModels;

    public class ChannelMessagesController : BaseApiController
    {
        public ChannelMessagesController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/channelMessages/{channelId}
        public IHttpActionResult GetAllChannelMessages(int channelId)
        {
            throw new NotImplementedException();
        }

        // POST api/channelMessages/{channelId}
        public IHttpActionResult PostChannelMessage(int channelId, ChannelMessageBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
