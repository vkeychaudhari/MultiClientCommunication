using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSignalRHub
{
    public class ChatHub : Hub
    {
        public void SendMessage(string user, string message)
        {
            Clients.All.broadcastMessage(user, message);
        }
    }
}
