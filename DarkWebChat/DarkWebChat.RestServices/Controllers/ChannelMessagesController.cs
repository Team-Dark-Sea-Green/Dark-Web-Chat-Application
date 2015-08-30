namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using DarkWebChat.RestServices.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    using Models.BindingModels;

    [RoutePrefix("api")]
    public class ChannelMessagesController : BaseApiController
    {
        public ChannelMessagesController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/channel-messages/{channelId}
        [HttpGet]
        [Route("channel-messages/{channelId}")]
        public IHttpActionResult GetAllChannelMessages(int channelId, [FromUri] string limit = null)
        {
            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Id == channelId);
            if (channel == null)
            {
                return this.NotFound();
            }

            IQueryable<ChannelMessage> messages = this.Data.ChannelMessages.All()
                .Where(m => m.Channel.Id == channel.Id)
                .OrderByDescending(m => m.Date)
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

            return this.Ok(
                messages.Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    Text = m.Content.Data,
                    DateSent = m.Date,
                    Sender = (m.Sender != null) ? m.Sender.UserName : null,
                    IsFile = m.Content.IsFile
                }));
        }

        // POST api/channel-messages/{channelId}
        [HttpPost]
        [Route("channel-messages/{channelId}")]
        public IHttpActionResult PostChannelMessage(int channelId, ChannelMessageBindingModel channelMessageData)
        {
            if (channelMessageData == null)
            {
                return this.BadRequest("Missing message data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = this.Data.Channels.All().FirstOrDefault(c => c.Id == channelId);
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

            MessageContent messageContent = new MessageContent
                                                {
                                                    Data = channelMessageData.Text,
                                                    IsFile = channelMessageData.IsFile != 0
                                                };

            this.Data.MessageContents.Add(messageContent);
            this.Data.SaveChanges();

            var message = new ChannelMessage()
                              {
                                  ContentId = messageContent.Id,
                                  ChannelId = channel.Id,
                                  Date = DateTime.Now,
                                  SenderId = currentUser.Id
                              };
            this.Data.ChannelMessages.Add(message);
            this.Data.SaveChanges();

            if (message.Sender == null)
            {
                return
                    this.Ok(
                        new
                            {
                                message.Id,
                                Message = "Anonymous message sent successfully to channel " + channel.Name + "."
                            });
            }

            return
                this.Ok(
                    new
                        {
                            message.Id,
                            Sender = message.Sender.UserName,
                            Message = "Message sent successfully to channel " + channel.Name + "."
                        });
        }
    }
}
