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
        public static void GetLocalAddress(Status status, string intenetAdress)
        {
            status.IpList = intenetAdress;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return;

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    status.CountMembers++;
                    status.IpList = ip.ToString();
                }
            }
        }
    }
}
