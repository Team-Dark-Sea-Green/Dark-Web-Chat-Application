namespace DarkWebChat.Web.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using DarkWebChat.Web.Models.BindingModels;
    using DarkWebChat.Web.Models.ViewModels;

    [Authorize]
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

        // GET api/channels/{channelName}
        [Route("channels/{channelName}")]
        public IHttpActionResult GetChannel(string channelName)
        {
            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channelName);

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

        // DELETE api/channels/{channelName}
        [HttpDelete]
        [Route("channels/{channelName}")]
        public IHttpActionResult DeleteChannel(string channelName)
        {
            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channelName);

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