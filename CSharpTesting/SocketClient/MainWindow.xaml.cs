using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private bool isOnMessageSetup {  get; set; } = false;

        public MainWindow()
        {
            //IF NOT RECOGNIZED BUT WORKING -> SWITCH TO RELEASE AND BACK TO DEBUG (SOLVED IT AT LEAST FOR NOW)
            InitializeComponent();
        }

        private void setup()
        {
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
            if (this.ws == null)
            {
                MessageBox.Show("Client is not connected");
            } else
            {
                this.ws!.Close();
            }
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