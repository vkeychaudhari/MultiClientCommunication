    using Microsoft.AspNetCore.Mvc;
using MVCSignalrClient.Models;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

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

            _connection.Headers.Add("ClientType", "WEB");
            _connection.Headers.Add("ClassName", "WEB Client");
            _connection.Headers.Add("IPAddress", getIPAddress());

            _hubProxy = _connection.CreateHubProxy("ChatHub");

            //_connection.Start();
        }

        static string LanOrWifi = "";
        public string getIPAddress()
        {
            string IP = "";
            string Operational_Status = "";
            try
            {
                //WiFi
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {

                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.IsDnsEligible)
                            {
                                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    IP = ip.Address.ToString();
                                    Operational_Status = ni.OperationalStatus.ToString();
                                    LanOrWifi = "WiFi";
                                }
                            }
                        }
                    }
                }
                if (IP != "")
                    IP += "-";
                //LAN
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                        if (addr != null && !addr.Address.ToString().Equals("0.0.0.0"))
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.IsDnsEligible)
                                {
                                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    {
                                        // All IP Address in the LAN
                                        IP += ip.Address.ToString();
                                        Operational_Status = ni.OperationalStatus.ToString();
                                        LanOrWifi = "LAN";
                                        //Console.WriteLine("My WIFI IP Address is :" + ip.Address.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                if (!IP.Contains("-"))
                    IP = "-" + IP;
            }
            catch (Exception ex)
            {
                IP = "Invalid Ip";
                //CommonHelper.WriteErrorLog(ex);
            }
            return IP;


            // // DebugHelper.writeDebugLog("ManageSenseClient FrmClient:- getIPAddress() calling from Start Client.");
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
