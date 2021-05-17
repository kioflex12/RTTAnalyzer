using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTAnalyser
{
    /// <summary>
    /// Локальная сеть 
    /// </summary>
    class LocalNetwork
    {
        private MainForm main;
       
        /// <summary>
        /// устанавливает список локальных адресов
        /// </summary>
        /// <param name="status"></param>
        /// <param name="intenetAdress"></param>
      

        public static async void SetIpList(MainForm mainForm, string intenetAdress, string mask = "192.168.0")
        {
            mainForm.Status.IpList = intenetAdress;
            for (int i = 2; i <= 255; i++)
            {
                string ipnum = $"{mask}.{i}";
                await Task.Run(() => PingCheck(ipnum, mainForm));

               
            }
            mainForm.GetStartButton.Enabled = true;
            mainForm.GetSetIpButton.Enabled = true;
            MessageBox.Show("сканирование завершено");
        }

        public static async void PingCheck(string A, MainForm mainForm)
        {
            await Task.Run(() =>
            {
                Ping ping = new Ping();
                PingReply pingReply = null;
                string server = A;
                pingReply = ping.Send(server);
                if (pingReply.Status == IPStatus.Success)
                {
                    MethodInvoker methodInvoker = delegate ()
                    {
                        mainForm.testListBox.Items.Add($"{server}");
                    };
                    mainForm.Invoke(methodInvoker);
                    
                }
            });
           
        }
    }
}
