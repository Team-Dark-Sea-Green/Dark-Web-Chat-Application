namespace DarkWebChat.RestServices.Controllers
{
    using System.Web.Http;

    using DarkWebChat.Data.UnitOfWork;

    using Data;

    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController(IDarkWebChatData data)
        {
            this.Data = data;
        }

        protected IDarkWebChatData Data { get; private set; }
    }
}
