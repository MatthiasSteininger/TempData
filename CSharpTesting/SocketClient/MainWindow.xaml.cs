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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace SocketClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocket? ws { get; set; }
        private bool isOnMessageSetup {  get; set; } = false;

        public MainWindow()
        {
            //IF NOT RECOGNIZED BUT WORKING -> SWITCH TO RELEASE AND BACK TO DEBUG (SOLVED IT AT LEAST FOR NOW)
            InitializeComponent();
        }

        private void setup()
        {
            string conString = "ws://" + this.tbxAddress.Text.ToString() + ":8080/WebSocketRoute";
            this.ws = new WebSocket(conString);

            lblMsg.Content = "WebSocket Setup Successfully";
        }

        private void btnCon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ws == null)
                {
                    setup();
                } else
                {
                    this.ws!.Close();
                    this.isOnMessageSetup = false; //dont need to remove old OnMessage Event since ws is new inited

                    setup();
                }

                if (this.isOnMessageSetup == false)
                {
                    this.ws!.OnMessage += (sender, e) => {
                        this.Dispatcher.BeginInvoke(new Action(() => {
                            lbxRecMessages.Items.Add("Client received a message: " + e.Data);
                        }));
                    };
                    this.isOnMessageSetup = true;
                }

                this.ws!.Connect();

                lblMsg.Content = "Tried Connecting Client";
            } catch (Exception ex)
            {
                MessageBox.Show("Exception thrown: \n MSG: " + ex.Message);
            }
        }

        private void btnDisCon_Click(object sender, RoutedEventArgs e)
        {
            this.ws!.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (this.ws == null || !this.ws!.IsAlive) {
                if (this.ws == null) {
                    MessageBox.Show("Client is Null and Connection is not Alive!");
                } else
                {
                    MessageBox.Show("Connection is not Alive!");
                }
            } else {
                this.ws!.Send("Hello, Server!");

                lblMsg.Content = "Sunt Message " + DateTime.Now.ToString();
            }
        }
    }
}