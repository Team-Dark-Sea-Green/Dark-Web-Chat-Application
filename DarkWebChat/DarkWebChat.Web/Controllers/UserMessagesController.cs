﻿namespace DarkWebChat.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;

    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using DarkWebChat.Web.Models.BindingModels;
    using DarkWebChat.Web.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    [Authorize]
    [RoutePrefix("api")]
    public class UserMessagesController : BaseApiController
    {
        public UserMessagesController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/user-messages/message/{id}
        [Route("user-messages/message/{id}")]
        [HttpGet]
        public IHttpActionResult GetUserMessage(int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var message = this.Data.UserMessages.All().FirstOrDefault(um => um.Id == id);

            if (message == null)
            {
                return this.NotFound();
            }

            if (message.RecieverId != loggedUserId && message.SenderId != loggedUserId)
            {
                return this.Unauthorized();
            }

            return
                this.Ok(
                    new
                        {
                            message.Id, 
                            message.Text, 
                            DateSent = message.Date, 
                            message.FileContent, 
                            Sender = (message.Sender != null) ? message.Sender.UserName : null, 
                            Reciever = (message.Reciever != null) ? message.Reciever.UserName : null
                        });
        }

        // GET api/user-messages/{username}
        [Route("user-messages/{username}")]
        [EnableQuery]
        [HttpGet]
        public IHttpActionResult GetAllUserMessages(string username, [FromUri] string limit = null)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            IQueryable<UserMessage> messages =
                this.Data.UserMessages.All()
                    .Where(
                        um =>
                        (um.RecieverId == loggedUserId && um.Sender.UserName == username)
                        || um.SenderId == loggedUserId && um.Reciever.UserName == username)
                    .OrderBy(m => m.Date)
                    .ThenByDescending(m => m.Id);

            if (limit != null)
            {
                int limitCount;
                int.TryParse(limit, out limitCount);
                if (limitCount >= 1 && limitCount <= 1000)
                {
                    messages = messages.Skip(Math.Max(0, messages.Count() - limitCount));
                }
                else
                {
                    return this.BadRequest("Limit should be integer in range [1..1000].");
                }
            }

            return this.Ok(messages.Select(UserMessageViewModel.Create));
        }

        // GET api/user-messages/users
        [Route("user-messages/users")]
        [HttpGet]
        public IHttpActionResult GetUserMessagesUsers()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var friends =
                this.Data.UserMessages.All()
                    .Where(m => m.SenderId == loggedUserId || m.RecieverId == loggedUserId)
                    .SelectMany(m => new[] { m.Reciever.UserName, m.Sender.UserName })
                    .Distinct().ToList();

            var loggedUser = this.Data.Users.Find(loggedUserId);
            var me = friends.FirstOrDefault(u => u == loggedUser.UserName);
            friends.Remove(me);

            return this.Ok(friends);
        }

        // POST api/user-messages/{username}
        [Route("user-messages/{username}")]
        [HttpPost]
        public IHttpActionResult PostUserMessage(string username, UserMessageBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var reciever = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (reciever == null)
            {
                return this.BadRequest("No such user.");
            }

            if (reciever == loggedUser)
            {
                return this.BadRequest("Can not send message to yourself.");
            }

            var message = new UserMessage
                              {
                                  Text = model.Text, 
                                  FileContent = model.FileContent, 
                                  SenderId = loggedUser.Id, 
                                  RecieverId = reciever.Id, 
                                  Date = DateTime.Now
                              };

            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();

            return this.Ok(UserMessageViewModel.CreateSingleView(message));
        }
    }
}