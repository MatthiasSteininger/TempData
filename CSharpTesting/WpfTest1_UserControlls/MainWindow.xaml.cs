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

namespace WpfTest1_UserControlls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainInstance;

        public MainWindow()
        {
            InitializeComponent();

            MainWindow.MainInstance = this;
            cbtnSend.setClickAction(CustomSendButtonAction);
        }

        private void btnClick1_Click(object sender, RoutedEventArgs e)
        {
            tbkOtherContent.Text = "Click Action has been setted";
            //over instance of Main Window Static does also work just for testing
        }

        private void CustomSendButtonAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICKER");
        }
    }
}
