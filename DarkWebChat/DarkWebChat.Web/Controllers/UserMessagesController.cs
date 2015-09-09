namespace DarkWebChat.Web.Controllers
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

            if (message.RecieverId != loggedUserId)
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
        [HttpGet]
        public IHttpActionResult GetAllUserMessages(string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var messages =
                this.Data.UserMessages.All()
                    .Where(
                        um =>
                        (um.RecieverId == loggedUserId && um.Sender.UserName == username)
                        || um.SenderId == loggedUserId && um.Reciever.UserName == username)
                    .Select(UserMessageViewModel.Create);

            return this.Ok(messages);
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