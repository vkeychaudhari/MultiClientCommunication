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

namespace WpfSignalRHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindEvents();
        }

        private void BindEvents()
        {
            // Bind the event handlers to the corresponding events
            ChatHub.ClientConnected += ChatHub_ClientConnected;
            ChatHub.ClientDisconnected += ChatHub_ClientDisconnected;
        }

        private void ChatHub_ClientDisconnected(string clientId, ClientInfo msClient)
        {
            Dispatcher.Invoke(() =>
            {
                // Add a new item to the connected client list when a client is disconnected
                lbConnectedClientList.Items.Add(msClient.className + " Disconnected" + " Type: " + msClient.clientType);
            });

        }

        private void ChatHub_ClientConnected(string clientId, ClientInfo msClient)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    // Add a new item to the connected client list when a client is connected
                    lbConnectedClientList.Items.Add(msClient.className + " Connected" + " Type: " + msClient.clientType);
                });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the event handling
            }
        }
    }
}
