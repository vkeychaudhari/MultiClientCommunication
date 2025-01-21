using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSignalrClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToHub();
        }

        private async void ConnectToHub()
        {
            _connection = new HubConnection("http://localhost:8080");
            
            _connection.Headers.Add("ClientType", "WPF");
            _connection.Headers.Add("ClassName", "WPF Client");
            _connection.Headers.Add("IPAddress", getIPAddress());

            _hubProxy = _connection.CreateHubProxy("ChatHub");

            _hubProxy.On<string, string>("broadcastMessage", (user, message) =>
            {
                Dispatcher.Invoke(() => MessagesListBox.Items.Add($"{user}: {message}"));
            });

            try
            {
                await _connection.Start();
                MessagesListBox.Items.Add("Connected to SignalR hub.");
            }
            catch (Exception ex)
            {
                MessagesListBox.Items.Add($"Connection failed: {ex.Message}");
            }
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

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (_connection.State == ConnectionState.Connected)
            {
                await _hubProxy.Invoke("SendMessage", "WPF Client", MessageTextBox.Text);
                MessageTextBox.Clear();
            }
            else
            {
                MessagesListBox.Items.Add("Not connected.");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Ensure you call SignalR disconnect logic
            _connection.Stop();
        }
    }
}
