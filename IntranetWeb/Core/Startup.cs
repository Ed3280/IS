using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
[assembly: OwinStartup(typeof(IntranetWeb.Core.Startup))]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace IntranetWeb.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {
            

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Auth/LogIn"),
                LogoutPath = new PathString("/Auth/LogOut"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30.0)
            });

            app.MapSignalR();
        }
    }
}