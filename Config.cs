using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MotorTown_Dedicated_Server_Handler
{
    public class Config
    {
        public string ServerName;
        public string MOTD;
        public string IP;
        public string Port;
        public string[] Admins;
        public int MaxPlayers;
        public bool AutoStart;
        public bool RestartOnCrash;
        public bool KeepUpdated;
        public int RestartFrequencyHours;
        public bool NoGUI;
        public string AccessKey = "???";
        public int Branch;

        public const string CONFIG_FILENAME = "config.xml";
        public static void SaveConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (TextWriter writer = new StreamWriter(CONFIG_FILENAME))
            {
                serializer.Serialize(writer, MainWindow.Config);
            }
            MainWindow.Logger.LogWithPrefix("Config", "Saved Config.");
        }

        public static void LoadConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            if(new FileInfo(CONFIG_FILENAME).Exists )
            {
                using (Stream reader = new FileStream(CONFIG_FILENAME, FileMode.Open))
                {
                    MainWindow.Config = (Config)serializer.Deserialize(reader);
                }
            }
            else
            {
                MainWindow.Config = new Config();
            }
        }
    }
}
