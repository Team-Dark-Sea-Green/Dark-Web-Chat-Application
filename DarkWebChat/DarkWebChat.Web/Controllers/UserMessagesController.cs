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

    public class UserMessagesController : BaseApiController
    {
        public UserMessagesController()
            : base(new DarkWebChatData())
        {
        }

        // GET api/user-messages/message/{id}
        [Route("api/user-messages/message/{id}")]
        [HttpGet]
        public IHttpActionResult GetUserMessage(int id)
        {
            var loggedUser = this.User.Identity.GetUserId();

            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var message = this.Data.UserMessages.All().FirstOrDefault(um => um.Id == id);

            if (message == null)
            {
                return this.NotFound();
            }

            if (message.RecieverId != loggedUser)
            {
                return this.Unauthorized();
            }

            return this.Ok(new
            {
                Id = message.Id,
                Text = message.Text,
                DateSent = message.Date,
                FileContent = message.FileContent,
                Sender = (message.Sender != null) ? message.Sender.UserName : null,
                Reciever = (message.Reciever != null) ? message.Reciever.UserName : null
            });
        }

        // GET api/user-messages/{username}
        [Route("api/user-messages/{username}")]
        [HttpGet]
        public IHttpActionResult GetAllUserMessages(string username)
        {
            var loggedUser = this.User.Identity.GetUserId();

            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            var messages =
                this.Data.UserMessages.All()
                    .Where(
                        um =>
                        (um.RecieverId == loggedUser && um.Sender.UserName == username)
                        || um.SenderId == loggedUser && um.Reciever.UserName == username)
                    .Select(MessageViewModel.Create);

            return this.Ok(messages);
        }

        // POST api/user-messages/{username}
        [Route("api/user-messages/{username}")]
        [HttpPost]
        public IHttpActionResult PostUserMessage(string username, UserMessageBindingModel model)
        {
            var loggedUser = this.User.Identity;

            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var reciever = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (reciever == null)
            {
                return this.BadRequest("No such user.");
            }

            var message = new UserMessage
                              {
                                  Text = model.Text, 
                                  FileContent = model.FileContent,
                                  SenderId = loggedUser.GetUserId(), 
                                  RecieverId = reciever.Id, 
                                  Date = DateTime.Now
                              };

            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();

            return
                this.Ok(
                    new MessageViewModel
                        {
                            Id = message.Id,
                            Text = message.Text,
                            ContainsFile = message.FileContent == null ? 0 : 1,
                            DateSent = message.Date,
                            Sender = loggedUser.GetUserName(),
                            Reciever = reciever.UserName,
                        });
        }
    }
}