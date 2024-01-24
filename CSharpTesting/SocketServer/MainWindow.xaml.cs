using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

using WebSocketSharp;
using WebSocketSharp.Server;

namespace SocketServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public static class GlobalUI
    {
        public static MainWindow? main { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            GlobalUI.main = this;
            InitializeComponent();

            //runs setup async
            Task.Run(() => {
                setup();
            });
        }

        private void setup()
        {
            var wssv = new WebSocketServer("ws://localhost:8080");
            wssv.AddWebSocketService<MyWebSocketBehavior>("/WebSocketRoute");
            wssv.Start();

            this.Dispatcher.BeginInvoke(new Action(() => {
                tbkMsg.Text = "WebSocket Server Started Successfully";
            }));

            // Example: Send a message to clients after 5 seconds
            //System.Threading.Thread.Sleep(5000);

            //MyWebSocketBehavior.SendMessageToClients("Hello, clients!");

            //Dispatcher.BeginInvoke(new Action(() => {
            //    tbkOtherContent.Text = "WebSocket Server Stopped...";
            //}));

            //wssv.Stop();
        }

        //Buttons
        private void btnSendToClients_Click(object sender, RoutedEventArgs e)
        {
            MyWebSocketBehavior.SendMessageToClients("Hello, clients!");
            //setup();
        }

        private void btnClick2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Other Things!");
        }
    }
}
