namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc.Html;
    using DarkWebChat.Data.UnitOfWork;
    using DarkWebChat.Models;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;

    public class UserMessagesController : BaseApiController
    {
        public UserMessagesController()
            :base(new DarkWebChatData())
        {
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

            var messages = this.Data.UserMessages.All()
                .Where(um => (um.RecieverId == loggedUser && um.Sender.UserName == username) ||
                             um.SenderId == loggedUser && um.Reciever.UserName == username)
                .Select(MessageViewModel.Create);

            return this.Ok(messages);
        }

        // POST api/user-messages/{username}
        [Route("api/user-messages/{username}")]
        [HttpPost]
        public IHttpActionResult PostUserMessage(string username, UserMessageBindingModel model)
        {
            var loggedUser = this.User.Identity.GetUserId();

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

            var message = new UserMessage()
            {
                Content = model.Content,
                IsFile = model.IsFile != 0,
                SenderId = loggedUser,
                RecieverId = reciever.Id,
                Date = DateTime.Now
            };

            this.Data.UserMessages.Add(message);
            this.Data.SaveChanges();

            return this.Ok(MessageViewModel.CreateSingleView(message));
        }
    }
}
