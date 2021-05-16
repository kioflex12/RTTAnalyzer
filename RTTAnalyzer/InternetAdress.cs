using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RTTAnalyser
{
    /// <summary>
    /// Интернет адресс из списка адрессов в IpConfig.json
    /// </summary>
    class InternetAdress
    {
        /// <summary>
        /// список соотвествия DNS и IP
        /// </summary>
        private Dictionary<string, string> dnsIpPair = new Dictionary<string, string>();
        /// <summary>
        /// Инциализации списка соотвествия между DNS и его IP адресса
        /// </summary>
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
