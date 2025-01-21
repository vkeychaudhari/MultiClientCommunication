using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.AspNet.SignalR;

namespace WpfSignalRHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }

    public class Server
    {
        private IDisposable _signalR;

        public void Start()
        {
            string url = "http://localhost:8080";
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };

            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(30);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(7);

            //GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10); // Default is 2 minutes
            //GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30); // Default is 30 seconds

            _signalR = WebApp.Start<Startup>(url);
            System.Console.WriteLine($"Server started at {url}");
        }

        public void Stop()
        {
            _signalR?.Dispose();
        }
    }
}
