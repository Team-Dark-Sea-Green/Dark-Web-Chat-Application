namespace DarkWebChat.Web.Controllers
{
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;

    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController(IDarkWebChatData data)
        {
            this.Data = data;
        }

        protected IDarkWebChatData Data { get; private set; }
    }
}