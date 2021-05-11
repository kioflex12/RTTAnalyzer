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
        private Thread _thread;
        private DirectoryController directoryController;
        private ThreadController _threadController;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            directoryController = new DirectoryController();
            directoryController.ClearTempDirectory();
            Status = new Status();
            _threadController = new ThreadController(Status, this);
            tabControl1.TabPages[0].Text = "Главная";
            tabControl1.TabPages[1].Text = "Status";
            InternetIpList.SelectedItem = InternetIpList.Items[0];

        }


        private void StartButton_Click(object sender, EventArgs e)
        {
            
            Status.GetIpList.Clear();
            ipArray.Columns.Clear();

            ChangeStateControllsButtons();
            LocalNetwork.GetLocalAddress(this);
            _threadController.StartThread();
           

        }

       private void ChangeStateControllsButtons()
        {
            StopButton.Enabled = !StopButton.Enabled;
            Thread.Sleep(1000);
            StartButton.Enabled = !StartButton.Enabled;
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

        private void StopButton_Click(object sender, EventArgs e)
        {
            _threadController.CloseThread();
            ChangeStateControllsButtons();
        }

        private void configLabel_Click(object sender, EventArgs e)
        {
            Process.Start("nmon.json");
        }

       
    }
}
