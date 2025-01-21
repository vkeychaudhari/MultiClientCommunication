    using Microsoft.AspNetCore.Mvc;
using MVCSignalrClient.Models;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;

namespace MVCSignalrClient.Controllers
{
    public class HomeController : Controller
    {
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _connection = new HubConnection("http://localhost:8080");
            _hubProxy = _connection.CreateHubProxy("ChatHub");
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            if (_connection.State == ConnectionState.Disconnected)
            {
                await _connection.Start();
            }

            await _hubProxy.Invoke("SendMessage", user, message);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
