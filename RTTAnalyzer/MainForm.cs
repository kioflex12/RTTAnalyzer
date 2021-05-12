using LiveCharts.Wpf;
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

namespace RTTAnalyser
{
    public partial class MainForm : Form
    {
        public DataGridView IpArray => ipArray;
        public Status Status;
        public Chart Chart;
        private DirectoryController directoryController;
        private ThreadController _threadController;
        private InternetAdress _internetAdress;

        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            directoryController = new DirectoryController();
            Status = new Status();
            _threadController = new ThreadController(Status, this);
            _internetAdress = new InternetAdress();
            Chart = new Chart(IpArray, cartesianChart, Status);
            directoryController.ClearTempDirectory();
            
            
            tabControl1.TabPages[0].Text = "Главная";
            tabControl1.TabPages[1].Text = "Status";
            tabControl1.TabPages[2].Text = "График";
            
            InitIpComboBox();
            InternetIpList.SelectedItem = InternetIpList.Items[0];



        }

        private void InitIpComboBox()
        {
            foreach (var dns in _internetAdress.GetAllDns())
            {
                InternetIpList.Items.Add(dns);

            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            
            Status.GetIpList.Clear();
            ipArray.Columns.Clear();

            ChangeStateControllsButtons();
            LocalNetwork.GetLocalAddress(Status,
                _internetAdress.GetIp(InternetIpList.SelectedItem.ToString()));
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
            intenetStatusTextBox.Text = Status.IntenetStatus.ToString();
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
