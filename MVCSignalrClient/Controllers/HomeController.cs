    using Microsoft.AspNetCore.Mvc;
using MVCSignalrClient.Models;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using MVCSignalrClient.Services;

namespace MVCSignalrClient.Controllers
{
    public class HomeController : Controller
    {
        private static ConcurrentDictionary<string, string> clientList = new ConcurrentDictionary<string, string>();
        
        private readonly ILogger<HomeController> _logger;

        //private static bool isConnectionInitialized = false;

        public HomeController(ILogger<HomeController> logger)
        {
            
        }

        [HttpGet]
        public IActionResult GetClientList()
        {
            if(SignalRServerClientHelper.Instance!=null)
                clientList = SignalRServerClientHelper.clientList;
            
            return Json(clientList);
        }

        // Displays the home page
        public IActionResult Index()
        {
            SignalRServerClientHelper.Instance.InitializeConnection();
            return View();
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
