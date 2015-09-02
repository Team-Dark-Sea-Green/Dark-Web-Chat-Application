namespace DarkWebChat.RestServices.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using DarkWebChat.Web.Controllers;
    using DarkWebChat.Web.Models.BindingModels;
    using DarkWebChat.Web.Models.ViewModels;

    [RoutePrefix("api")]
    public class ChannelsController : BaseApiController
    {
        public ChannelsController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/channels
        [Route("channels")]
        public IHttpActionResult GetAllChannels()
        {
            var allChannels = this.Data.Channels.All().OrderBy(c => c.Name).Select(c => new { c.Id, c.Name });

            return this.Ok(allChannels);
        }

        // GET api/channels/{id}
        [Route("channels/{id}")]
        public IHttpActionResult GetChannel(int id)
        {
            var channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ChannelViewModel { Id = channel.Id, Name = channel.Name });
        }

        // POST api/channels
        [Route("channels")]
        public IHttpActionResult CreateChannel(ChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Empty channel data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Channels.All().Any(c => c.Name == model.Name))
            {
                this.Content(HttpStatusCode.Conflict, new { Message = "Duplicated channel name: " + model.Name });
            }

            var channel = new Channel { Name = model.Name };
            this.Data.Channels.Add(channel);
            this.Data.SaveChanges();

            return this.Ok(new ChannelViewModel { Id = channel.Id, Name = channel.Name });
        }

        // PUT api/channels/adduser
        [HttpPut]
        [Route("channels/adduser")]
        public IHttpActionResult AddUserToChannel(AddUserToChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = this.Data.Channels.Find(model.channelId);
            if (channel == null)
            {
                return this.BadRequest("There is no such channel!");
            }

            var user = this.Data.Users.Find(model.userlId);
            if (user == null)
            {
                return this.BadRequest("There is no such user!");
            }

            channel.Users.Add(user);
            this.Data.Channels.Update(channel);
            this.Data.SaveChanges();

            return this.Ok(new ChannelViewModel { Id = channel.Id, Name = channel.Name, Users = channel.Users });
        }

        // DELETE api/channels/{id}
        [HttpDelete]
        [Route("channels/{id}")]
        public IHttpActionResult DeleteChannel(int id)
        {
            var channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return this.BadRequest("There is no such channel!");
            }

            this.Data.Channels.Remove(channel);
            this.Data.SaveChanges();

            return this.Ok(new { Message = "Channel " + channel.Name + " deleted!" });
        }
    }
}