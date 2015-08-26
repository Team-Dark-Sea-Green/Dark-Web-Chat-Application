namespace DarkWebChat.RestServices.Controllers
{
    using System.Web.Http;
    using Data;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            this.Data = new WebChatContext();
        }

        public WebChatContext Data { get; set; }
    }
}
