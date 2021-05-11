using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public DataGridView IpArray => ipArray;
        public Status Status;

        public MainForm()
        {
            DirectoryController directoryController = new DirectoryController();
            directoryController.ClearTempDirectory();
            Status = new Status();
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
            Unalyzer.InitAnylyzer(Status.GetIpList, this);
        }
        
        public void UpdateStatusSummary()
        {
            AvgPingTextBox.Text = Status.AvgPing.ToString();
            MaxPingTextBox.Text = Status.MaxPing.ToString();
            NetworkStatusTextBox.Text = Status.NetworkStatus.ToString();
            CountMembersTextBox.Text = Status.CountMembers.ToString();
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(botLink.Text);
        }
    }
}
