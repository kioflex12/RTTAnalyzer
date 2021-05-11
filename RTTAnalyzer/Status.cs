using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTAnalyzer
{
    public class Status
    {
        private List<string> _ipList = new List<string>() { };
        private int _countMembers = 0;
        private int _avgPing = 0;
        private string _networkStatus = "bad";
        private int _maxPing = 0;


        public string IpList { set => _ipList.Add(value); }
        public List<string> GetIpList => _ipList;
        public int CountMembers { get => _countMembers; set => _countMembers = value; }
        public int AvgPing { get => _avgPing; set => _avgPing = value; }
        public int MaxPing { get => _maxPing; set => _maxPing = value; }
        public string NetworkStatus { get => _networkStatus; set => _networkStatus = value; }
    }
}
