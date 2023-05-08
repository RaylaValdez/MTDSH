using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

// TIMBERRRRRRRRRRRRRRRRRRRRRRRRRRR

namespace MotorTown_Dedicated_Server_Handler
{
    public class Logger
    {
        TextBox outputTextbox;
        Dictionary<string, string> prefixedBuffers = new();

        public Logger(TextBox textbox)
        {
            outputTextbox = textbox;
        }
        public void Log(string str)
        {
            Console.WriteLine(str);
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                outputTextbox.AppendText(str);
            });
        }

        public void LogWithPrefix(string prefix, string str)
        {
            if(!prefixedBuffers.ContainsKey(prefix))
            {
                prefixedBuffers[prefix] = "";
            }
            prefixedBuffers[prefix] += str;

            string[] split = prefixedBuffers[prefix].Split("\n");
            if (split.Length > 1)
            {

                string time = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");
                for (int i = 0; i < split.Length; i++)
                {
                    string s = split[i];
                    if (i == 0)
                        Log(time + ": [" + prefix + "] ");
                    Log(s);
                    if (i == split.Length && !s.EndsWith("\n"))
                        Log("\n");

                }
                prefixedBuffers[prefix] = "";
            }
        }
    }
}
