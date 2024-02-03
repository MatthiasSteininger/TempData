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
using System.Windows.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SocketServer
{
    class MyWebSocketBehavior : WebSocketBehavior
    {
        private static readonly List<MyWebSocketBehavior> Clients = new List<MyWebSocketBehavior>();

        protected override void OnOpen()
        {
            // Add the connected client to the list
            Clients.Add(this);

            //invoke mainwindow dispatcher to execute this action as soons as possible on this thread (as i understand it)
            GlobalUI.main!.Dispatcher.BeginInvoke(new Action(() => {
                GlobalUI.main!.lblConnectedClientsao.Content = Clients.Count.ToString();
            }));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            // Remove the disconnected client from the list
            Clients.Remove(this);

            //invoke mainwindow dispatcher to execute this action as soons as possible on this thread (as i understand it)
            GlobalUI.main!.Dispatcher.BeginInvoke(new Action(() => {
                GlobalUI.main!.lblConnectedClientsao.Content = Clients.Count.ToString();
            }));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            GlobalUI.main!.Dispatcher.BeginInvoke(new Action(() =>
            {
                GlobalUI.main!.lbxMsgs.Items.Add(e.Data);
            }));
        }

        // Method to send a message to all connected clients
        public static void SendMessageToClients(string message)
        {
            if (Clients.Count == 0) {
                MessageBox.Show("There are no Clients Connected");
            } else
            {
                try
                {
                    foreach (var client in Clients)
                    {
                        client.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending message to a client: " + ex.Message);
                }
            }
        }
    }
}
