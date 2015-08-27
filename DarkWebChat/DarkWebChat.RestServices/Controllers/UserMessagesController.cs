namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;

    using Models.BindingModels;

    public class UserMessagesController : BaseApiController
    {
        public UserMessagesController()
            :base(new DarkWebChatData())
        {
            
        }
        // GET api/userMessages/{userId}
        public IHttpActionResult GetAllUserMessages(int userId)
        {
            throw new NotImplementedException();
        }

        // POST api/userMessages/{userId}
        public IHttpActionResult PostUserMessage(int userId, UserMessageBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}
