using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTAnalyzer
{
    public partial class MainForm : Form
    {
        private List<string> _ipList = new List<string>() {};
        public DataGridView IpArray => ipArray;

        public string IpList { set => _ipList.Add(value); }

        public MainForm()
        {
            DirectoryController directoryController = new DirectoryController();
            directoryController.ClearTempDirectory();
            InitializeComponent();
            tabControl1.TabPages[0].Text = "Главная";
            tabControl1.TabPages[1].Text = "Status";

        }

        
        private void StartButton_Click(object sender, EventArgs e)
        {
            LocalNetwork.GetLocalAddress(this);
            Thread task = new Thread(StartAsync);
            task.Start();

        }
        
        private void StartAsync()
        {
            Unalyzer.InitAnylyzer(_ipList, this);
        }

        
        private void OnFormClosing(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
