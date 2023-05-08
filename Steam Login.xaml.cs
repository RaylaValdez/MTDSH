using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MotorTown_Dedicated_Server_Handler
{
    /// <summary>
    /// Interaction logic for Steam_Login.xaml
    /// </summary>
    public partial class Steam_Login : Window
    {
        public Steam_Login()
        {
            InitializeComponent();
        }

        private void login_button_Click(object sender, RoutedEventArgs e)
        {
            string logincmd = "+login " + steam_user + " " + steam_password + " " + steam_guardcode;
            this.Close();
        }
    }
}
