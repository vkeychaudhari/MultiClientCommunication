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
            // Define the URL for the server
            string url = "http://localhost:8080";

            // Configure the SignalR hub
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };

            // Set the connection timeout and disconnect timeout
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(30);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(7);

            GlobalHost.Configuration.KeepAlive = new TimeSpan(0, 0, 2);

            // Start the SignalR server with the specified URL and configuration
            _signalR = WebApp.Start<Startup>(url);

            // Print the server URL to the console
            System.Console.WriteLine($"Server started at {url}");
        }

        public void Stop()
        {
            _signalR?.Dispose();
        }
    }
}
