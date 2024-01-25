using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
        private bool isOnMessageSetuped {  get; set; } = false;
        private bool isCurrentlyTryingToConnect { get; set; } = false;

        public MainWindow()
        {
            //IF NOT RECOGNIZED BUT WORKING -> SWITCH TO RELEASE AND BACK TO DEBUG (SOLVED IT AT LEAST FOR NOW)
            InitializeComponent();
        }

        private void setup()
        {
            this.isCurrentlyTryingToConnect = true;

            string tbxAddress_Content = "";
            this.Dispatcher.InvokeAsync(new Action(() => {
                //Thread.Sleep(1000); //sends the MainThread to sleep since this action is executed on the main thread via the Invoke
                tbxAddress_Content = this.tbxAddress.Text.ToString();
            })).Wait();
            //waiting for the action to finish

            string conString = "ws://" + tbxAddress_Content + ":8080/WebSocketRoute";
            this.Dispatcher.BeginInvoke(() =>
            {
                this.lblMsg.Content = "TRYING: " + conString;
            });

            var ws = new WebSocket(conString);

            //is another option to wait for the task to finish
            TaskCompletionSource<bool> tcs_wsSetup = new TaskCompletionSource<bool>();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.ws = ws;
                tcs_wsSetup.SetResult(true);
            }));
            tcs_wsSetup.Task.Wait();

            this.isCurrentlyTryingToConnect = false;
        }

        private void btnCon_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => {
                try
                {
                    if (this.ws == null)
                    {
                        setup();
                    }
                    else
                    {
                        this.ws!.Close();
                        this.isOnMessageSetuped = false; //dont need to remove old OnMessage Event since ws is new inited

                        setup();
                    }

                    if (this.isOnMessageSetuped == false)
                    {
                        this.ws!.OnMessage += (sender, e) => {
                            this.Dispatcher.BeginInvoke(new Action(() => {
                                lbxRecMessages.Items.Add("Client received a message: " + e.Data);
                            }));
                        };
                        this.isOnMessageSetuped = true;
                    }

                    this.ws!.Connect();

                    this.Dispatcher.BeginInvoke(new Action(() => {
                        if (this.ws!.IsAlive)
                        {
                            lblMsg.Content = "Connection is alive";
                        } else
                        {
                            lblMsg.Content = "Connecting Client failed";
                        }
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception thrown: \n MSG: " + ex.Message);
                }
            });
        }

        private void btnDisCon_Click(object sender, RoutedEventArgs e)
        {
            if (this.ws != null && this.ws.IsAlive && !this.isCurrentlyTryingToConnect)
            {
                this.ws!.Close();
            } else
            {
                MessageBox.Show("Client is not connected or currently trying to");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (this.ws == null || !this.ws!.IsAlive) {
                MessageBox.Show("Client is not connected or currently trying to");
            } else {
                this.ws!.Send("Hello, Server!");

                lblMsg.Content = "Sunt Message " + DateTime.Now.ToString();
            }
        }
    }
}