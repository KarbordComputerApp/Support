using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System.IO;
using System.Web.Hosting;
using System;

[assembly: OwinStartup(typeof(Support.Startup))]

namespace Support
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}