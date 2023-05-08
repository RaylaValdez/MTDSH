using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotorTown_Dedicated_Server_Handler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Config Config;
        public static Logger Logger;
        public static MainWindow Instance;

        bool ServerRunning = false;
        Process ServerProcess;
        private const string STEAMCMD_DIR = "steamcmd";
        private const string STEAMCMD_ZIP = "temp.zip";
        private static readonly string STEAMCMD_PATH = $"{STEAMCMD_DIR}\\steamcmd.exe";
        private static readonly string RUNSCRIPT_PATH = $"{STEAMCMD_DIR}\\runscript.txt";
        private const string RUNSCRIPT_BETA = "force_install_dir ../\nset_app_beta_password 2223650 -betapassword %PWD%\napp_update 2223650\nquit";
        private const string RUNSCRIPT = "force_install_dir ../\napp_update 2223650\nquit";
        public string LOGIN_VAR = "+login anonymous";

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            var workingDir = new FileInfo(typeof(MainWindow).Assembly.Location).Directory.ToString();
            var binDir = System.IO.Path.Combine(workingDir, "MotorTownDedicated");
            Directory.SetCurrentDirectory(workingDir);

            Logger = new Logger(this.consolelog);
            Logger.LogWithPrefix("System", "Logger init\n");

            Config.LoadConfig();
            if (Config.Branch != 0 && Config.AccessKey == "???")
            {
                OpenAccessKeyPopup();
            }
            UpdateConfigOnScreen();

            // if config has update flag checked, run steamCMD and tell it to update
            // log all steam CMD does and Send it to ConsoleLog
            // TODO add config
            if (!Config.KeepUpdated)
            {
                return;
            }

            UpdateApp();
        }

        private void OpenAccessKeyPopup()
        {
            AccessKeyPopup accessKeyPopup = new AccessKeyPopup();
            accessKeyPopup.ShowDialog();
        }

        private void UpdateApp()
        {
            Thread thread = new Thread(new ThreadStart(this.StartSteamCMD));
            thread.IsBackground = true;
            thread.Start();
        }

        private void StartSteamCMD()
        {
            if (!Directory.Exists(STEAMCMD_DIR))
            {
                Directory.CreateDirectory(STEAMCMD_DIR);
            }

            if (Config.Branch == 0)
                File.WriteAllText(RUNSCRIPT_PATH, RUNSCRIPT);
            else
                File.WriteAllText(RUNSCRIPT_PATH, RUNSCRIPT_BETA.Replace("%PWD%", Config.AccessKey));

            this.Dispatcher.Invoke(() => MainTabs.SelectedIndex = 0);

            if (!File.Exists(STEAMCMD_PATH))
            {
                try
                {
                    Logger.LogWithPrefix("Info", "Downloading SteamCMD.\n");
                    using (var client = new WebClient())
                        client.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", STEAMCMD_ZIP);

                    ZipFile.ExtractToDirectory(STEAMCMD_ZIP, STEAMCMD_DIR);
                    File.Delete(STEAMCMD_ZIP);
                    Logger.LogWithPrefix("Info", "SteamCMD downloaded successfully!\n");
                }
                catch (Exception e)
                {
                    Logger.LogWithPrefix("Error","Failed to download SteamCMD, unable to update the DS.\n");
                    Logger.LogWithPrefix("Error", e.Message);
                    Logger.Log("\n");
                    return;
                }
            }

            Logger.LogWithPrefix("Info", "Checking for DS updates.");
            var steamCmdProc = new ProcessStartInfo(STEAMCMD_PATH, "+runscript runscript.txt")
            {
                WorkingDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), STEAMCMD_DIR),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.ASCII,
                CreateNoWindow = true,
            };
            var cmd = Process.Start(steamCmdProc);

            while (!cmd.HasExited)
            {
                Logger.LogWithPrefix("Info", cmd.StandardOutput.ReadToEnd());
                Thread.Sleep(100);
            }
            Logger.LogWithPrefix("Info", "Completed SteamCMD update\n");
        }

        private void UpdateConfigOnScreen()
        {
            config_autostart.IsChecked = Config.AutoStart;
            config_update.IsChecked = Config.KeepUpdated;
            config_maxplayers.Text = Config.MaxPlayers.ToString();
            config_nogui.IsChecked = Config.NoGUI;
            config_ip.Text = Config.IP;
            config_motd.Text = Config.MOTD;
            config_port.Text = Config.Port;
            config_restartoncrash.IsChecked = Config.RestartOnCrash;
            config_restartschedule.Value = Config.RestartFrequencyHours;
            config_servername.Text = Config.ServerName;
            config_restartfreqlabel.Content = Config.RestartFrequencyHours;
            if (Config.Admins != null)
                config_admins.Text = string.Join("\n", Config.Admins);
            config_branch.SelectedIndex = Config.Branch;
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void config_savebutton_Click(object sender, RoutedEventArgs e)
        {
            Config.SaveConfig();
        }

        private void config_autostart_clicked(object sender, RoutedEventArgs e)
        {
            Config.SaveConfig();
        }

        private void config_update_click(object sender, RoutedEventArgs e)
        {
            Config.KeepUpdated = config_update.IsChecked ?? false;
            Config.SaveConfig();
        }

        private void branch_key_button_click(object sender, RoutedEventArgs e)
        {
            OpenAccessKeyPopup();
        }

        private void update_button_click(object sender, RoutedEventArgs e)
        {
            UpdateApp();
        }
    }
}
