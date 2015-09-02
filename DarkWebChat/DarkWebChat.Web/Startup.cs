using DarkWebChat.Web;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DarkWebChat.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}