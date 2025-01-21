using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

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
            _signalR = WebApp.Start<Startup>(url);
            System.Console.WriteLine($"Server started at {url}");
        }

        public void Stop()
        {
            _signalR?.Dispose();
        }
    }
}
