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

namespace WpfTest1_UserControlls.UserControlls
{
    /// <summary>
    /// Interaktionslogik für CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        public void setClickAction(RoutedEventHandler clickAction)
        //delegate that more or less promises that he is capable of in this case click action
        //so that compiler can type check - SAFETY
        {
            this.btnSend.Click += clickAction;
        }
    }
}
