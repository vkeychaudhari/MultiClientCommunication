using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSignalRHub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Server _server;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _server = new Server();
            _server.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _server.Stop();
            base.OnExit(e);
        }
    }
}
