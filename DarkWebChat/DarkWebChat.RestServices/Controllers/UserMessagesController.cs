namespace DarkWebChat.RestServices.Controllers
{
    using System;
    using System.Web.Http;
    using Models.BindingModels;

    public class UserMessagesController : BaseApiController
    {
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
