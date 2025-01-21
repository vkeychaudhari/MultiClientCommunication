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

        // Connect to the SignalR hub
        private async void ConnectToHub()
        {
            // Create a new HubConnection with the specified URL
            _connection = new HubConnection("http://localhost:8080");

            // Add custom headers to the connection
            _connection.Headers.Add("ClientType", "WPF");
            _connection.Headers.Add("ClassName", "WPF Client");
            _connection.Headers.Add("IPAddress", getIPAddress());

            // Create a new HubProxy with the specified hub name
            _hubProxy = _connection.CreateHubProxy("ChatHub");

            // Register a callback for the "broadcastMessage" event
            _hubProxy.On<string, string>("broadcastMessage", (user, message) =>
            {
                // Update the UI with the received message
                Dispatcher.Invoke(() => MessagesListBox.Items.Add($"{user}: {message}"));
            });

            try
            {
                // Start the connection to the hub
                await _connection.Start();
                MessagesListBox.Items.Add("Connected to SignalR hub.");
            }
            catch (Exception ex)
            {
                MessagesListBox.Items.Add($"Connection failed: {ex.Message}");
            }
        }

        // Get the IP address of the client
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
            }
            return IP;
        }

        // Send a message to the SignalR hub
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

        // Handle the window closed event
        private void Window_Closed(object sender, EventArgs e)
        {
            // Ensure you call SignalR disconnect logic
            _connection.Stop();
        }
    }
}
