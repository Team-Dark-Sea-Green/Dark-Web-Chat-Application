namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using DarkWebChat.Web.Controllers;
    using DarkWebChat.Web.Models.BindingModels;
    using DarkWebChat.Web.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    [RoutePrefix("api")]
    public class ChannelMessagesController : BaseApiController
    {
        public ChannelMessagesController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/channel-messages/{channelName}
        [HttpGet]
        [Route("channel-messages/{channelName}")]
        public IHttpActionResult GetAllChannelMessages(string channelName, [FromUri] string limit = null)
        {
            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channelName);
            if (channel == null)
            {
                return this.NotFound();
            }

            IQueryable<ChannelMessage> messages =
                this.Data.ChannelMessages.All()
                    .Where(m => m.Channel.Id == channel.Id)
                    .OrderBy(m => m.Date)
                    .ThenByDescending(m => m.Id);

            if (limit != null)
            {
                int limitCount;
                int.TryParse(limit, out limitCount);
                if (limitCount >= 1 && limitCount <= 1000)
                {
                    messages = messages.Take(limitCount);
                }
                else
                {
                    return this.BadRequest("Limit should be integer in range [1..1000].");
                }
            }

            return
                 this.Ok(
                     messages.Select(
                         m =>
                         new MessageViewModel
                         {
                             Id = m.Id,
                             Content = m.Content,
                             DateSent = m.Date,
                             Sender = (m.Sender != null) ? m.Sender.UserName : null,
                             FileContent = m.FileContent
                         }));
        }

        // POST api/channel-messages/{channelName}
        [HttpPost]
        [Route("channel-messages/{channelName}")]
        public IHttpActionResult PostChannelMessage(string channelName, ChannelMessageBindingModel channelMessageData)
        {
            if (channelMessageData == null)
            {
                return this.BadRequest("Missing message data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Name == channelName);
            if (channel == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            if (currentUser == null)
            {
                return this.BadRequest("Login to send message.");
            }

            var message = new ChannelMessage
                              {
                                  Content = channelMessageData.Content, 
                                  FileContent = channelMessageData.FileContent, 
                                  ChannelId = channel.Id, 
                                  Date = DateTime.Now, 
                                  SenderId = currentUser.Id
                              };
            this.Data.ChannelMessages.Add(message);
            this.Data.SaveChanges();
            
            return
                this.Ok(new MessageViewModel
                {
                    Id = message.Id,
                    Content = message.Content,
                    DateSent = message.Date,
                    Sender = message.Sender.UserName,
                    FileContent = message.FileContent
                });
        }
    }
}