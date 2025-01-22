    using Microsoft.AspNetCore.Mvc;
using MVCSignalrClient.Models;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;

namespace MVCSignalrClient.Controllers
{
    public class HomeController : Controller
    {
        private static ConcurrentDictionary<string, string> clientList = new ConcurrentDictionary<string, string>();
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        private readonly ILogger<HomeController> _logger;

        private static bool isConnectionInitialized = false;

        public HomeController(ILogger<HomeController> logger)
        {
            //_logger = logger;
            //_connection = new HubConnection("http://localhost:8080");

            //_connection.Headers.Add("ClientType", "WEB");
            //_connection.Headers.Add("ClassName", "WEB Client");
            ////_connection.Headers.Add("IPAddress", getIPAddress());

            //_hubProxy = _connection.CreateHubProxy("ChatHub");

            //// Register a callback for the "getClientList" event
            //_hubProxy.On<ConcurrentDictionary<string, string>>("getClientList", (_clientList) =>
            //{
            //    clientList = _clientList;
            //    // Update the UI with the received message
            //    //Dispatcher.Invoke(() => MessagesListBox.Items.Add($"{user}: {message}"));
            //});

            //_connection.Start();
        }

        [HttpGet]
        public IActionResult GetClientList()
        {
            return Json(clientList);
        }

        [HttpPost]
        // Sends a message to the server
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            if (_connection.State == ConnectionState.Disconnected)
            {
                await _connection.Start();
            }

            await _hubProxy.Invoke("SendMessage", user, message);
            return RedirectToAction("Index");
        }

        // Displays the home page
        public IActionResult Index()
        {
            InitializeConnection();
            //if (HttpContext.Session.GetString("ConnectionInitialized") != "true")
            //{
            //    InitializeConnection();
            //    HttpContext.Session.SetString("ConnectionInitialized", "true");
            //}
            return View();
        }

        private void InitializeConnection()
        {
            if(_connection != null) _connection.Stop();
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

        // Displays the privacy page
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // Displays the error page
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
