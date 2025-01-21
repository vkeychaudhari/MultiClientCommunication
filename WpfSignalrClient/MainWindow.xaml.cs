using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
