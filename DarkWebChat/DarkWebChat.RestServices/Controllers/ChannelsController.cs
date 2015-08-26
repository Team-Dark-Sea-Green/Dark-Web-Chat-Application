namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Authentication.ExtendedProtection;
    using System.Web.Http;
    using Models.BindingModels;

    public class ChannelsController : BaseApiController
    {
        // GET api/channels
        public IHttpActionResult GetAllChannels()
        {
            throw new NotImplementedException();
        }

        // GET api/channels/{id}
        public IHttpActionResult GetChannel(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/channels
        public IHttpActionResult CreateChannel(ChannelBindingModel model)
        {
            throw new NotImplementedException();
        }

        //Add user to channel

        // DELETE api/channels/{id}
        public IHttpActionResult DeleteChannel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
