using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RTTAnalyser
{
    class LocalNetwork
    {
        public static void GetLocalAddress(MainForm mainForm)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return;

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    mainForm.Status.CountMembers++;
                    mainForm.Status.IpList = ip.ToString();
                }
            }
        }
    }
}
