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
        public WebSocketServer? wssv { get; set; } = null;

        public MainWindow()
        {
            GlobalUI.main = this;
            InitializeComponent();
        }

        private void buildSocket()
        {
            try
            {
                string tbxContent = this.tbxSetupString.Text;
                if (this.wssv == null)
                {
                    Task.Run(() => {
                        this.wssv = new WebSocketServer("ws://" + tbxContent + ":8080");
                        this.wssv.AddWebSocketService<MyWebSocketBehavior>("/WebSocketRoute");
                        this.wssv.Start();

                        this.Dispatcher.BeginInvoke(new Action(() => {
                            tbkMsg.Text = "WebSocket Setup with " + tbxContent;
                            btnSetup.Content = "ReSetup";
                        }));
                    });
                }
                else
                {
                    Task.Run(() => {
                        this.wssv.RemoveWebSocketService("/WebSocketRoute");
                        this.wssv.Stop();
                        this.wssv = null;

                        this.wssv = new WebSocketServer("ws://" + tbxContent + ":8080");
                        this.wssv.AddWebSocketService<MyWebSocketBehavior>("/WebSocketRoute");
                        this.wssv.Start();

                        this.Dispatcher.BeginInvoke(new Action(() => {
                            tbkMsg.Text = "WebSocket Setup with " + tbxContent;
                        }));
                    });
                }
            } catch (Exception ex)
            {
                MessageBox.Show("There has been an Exception while Setting up the Web Socket Server!\nExcMSG: " + ex.Message);
            }
        }

        private void btnSendToClients_Click(object sender, RoutedEventArgs e)
        {
            //static method should not break
            if (this.wssv != null)
            {
                MyWebSocketBehavior.SendMessageToClients("Hello, clients!");
            } else
            {
                MessageBox.Show("The Socket Server is not running!");
            }
        }

        private void btnSetup_Click(object sender, RoutedEventArgs e)
        {
            buildSocket();
        }
    }
}
