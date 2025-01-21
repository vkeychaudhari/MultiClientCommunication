using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WpfSignalRHub
{
    public delegate void ClientConnectionEventHandler(string clientId, ClientInfo msClient);

    public class ChatHub : Hub
    {
        public static ConcurrentDictionary<string, string> _allClients = new ConcurrentDictionary<string, string>();
        public static event ClientConnectionEventHandler ClientConnected;
        public static event ClientConnectionEventHandler ClientDisconnected;

        public override Task OnConnected()
        {
            ClientInfo etClient = PopulateClient("Connected");
            _allClients.TryAdd(Context.ConnectionId, etClient.className);
            ClientConnected?.Invoke(Context.ConnectionId, etClient);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ClientInfo etClient = PopulateClient("DisConnected");
            string classname;
            _allClients.TryRemove(Context.ConnectionId, out classname);
            ClientDisconnected?.Invoke(Context.ConnectionId, etClient);
            return base.OnDisconnected(stopCalled);
        }

        private ClientInfo PopulateClient(string _Status)
        {
            ClientInfo etClient = new ClientInfo();

            //if (Context.Headers["ClientType"] == "WPF")
            //{

            //}

            if (Context.Headers["ClientType"] != null)
                etClient.clientType = Context.Headers["ClientType"];

            if (Context.Headers["ClassName"] != null)
                etClient.className = Context.Headers["ClassName"];
            
            if (Context.Headers["IPAddress"] != null)
                etClient.clientIP_Address = Context.Headers["IPAddress"];

            switch (_Status)
            {
                case "Connected":
                    etClient.currentStatus = "ON";
                    break;
                case "DisConnected":
                    etClient.currentStatus = "OFF";
                    break;
            }
            return etClient;
        }

        public void SendMessage(string user, string message)
        {
            Clients.All.broadcastMessage(user, message);
        }
    }

    public class ClientInfo
    {
        public string clientType;
        public string className;
        public string macAddress;
        public string clientIP_Address;
        public string connectionId { get; set; }
        public string clientPanelId { get; set; }
        public string currentStatus { get; set; }
    }
}
