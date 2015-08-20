using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DarkWebChat.RestServices.Startup))]

namespace DarkWebChat.RestServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Add the SignalR Middlewware to the OWIN pipeline
            app.MapSignalR();
        }
    }
}
