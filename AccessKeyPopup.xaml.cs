using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MotorTown_Dedicated_Server_Handler
{
    /// <summary>
    /// Interaction logic for AccessKeyPopup.xaml
    /// </summary>
    public partial class AccessKeyPopup : Window
    {
        public AccessKeyPopup()
        {
            InitializeComponent();
            UpdateConfigOnScreen();
        }

        private void config_save_key_button_click(object sender, RoutedEventArgs e)
        {
            MainWindow.Config.AccessKey = config_keyentry.Text;
            Config.SaveConfig();
            this.Close();
        }

        private void UpdateConfigOnScreen()
        {
            config_keyentry.Text = MainWindow.Config.AccessKey;
        }

    }
}
