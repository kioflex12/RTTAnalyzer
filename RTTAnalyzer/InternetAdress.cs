using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTAnalyser
{
    class InternetAdress
    {
        private Dictionary<string, string> dnsIpPair = new Dictionary<string, string>();

        public InternetAdress()
        {
            dnsIpPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("IpConfig.json"));
        }

        public string GetIp(string dns)
        {
            return dnsIpPair[dns];
        }

        public List<string> GetAllDns()
        {
            return dnsIpPair.Keys.ToList();
        }
    }
}
