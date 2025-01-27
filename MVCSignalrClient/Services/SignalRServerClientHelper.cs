using Microsoft.AspNet.SignalR.Client;
using System.Collections.Concurrent;

namespace MVCSignalrClient.Services
{
    public class SignalRServerClientHelper
    {
        public static ConcurrentDictionary<string, string> clientList = new ConcurrentDictionary<string, string>();
        private IHubProxy _hubProxy;
        private HubConnection _connection;
        // Private static readonly instance, initialized lazily
        private static readonly Lazy<SignalRServerClientHelper> _instance = new Lazy<SignalRServerClientHelper>(() => new SignalRServerClientHelper());

        // Private constructor to prevent instantiation from outside
        public SignalRServerClientHelper()
        {
            // Initialization logic if any
        }

        // Public static property to get the single instance of the class
        public static SignalRServerClientHelper Instance => _instance.Value;

        public void InitializeConnection()
        {
            if (_connection != null) 
            {
                _connection.Stop();
                _connection.Dispose();
            } 

            _connection = new HubConnection("http://localhost:8080");

            _connection.Headers.Add("ClientType", "WEB");
            _connection.Headers.Add("ClassName", "WEB Client");
            //_connection.Headers.Add("IPAddress", getIPAddress());

            _hubProxy = _connection.CreateHubProxy("ChatHub");

            // Register a callback for the "getClientList" event
            _hubProxy.On<ConcurrentDictionary<string, string>>("getClientList", (_clientList) =>
            {
                clientList = _clientList;
                // Update the UI with the received message
                //Dispatcher.Invoke(() => MessagesListBox.Items.Add($"{user}: {message}"));
            });

            _connection.Start();
        }
    }
}
