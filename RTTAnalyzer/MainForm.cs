using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace RTTAnalyser
{
    public partial class MainForm : Form
    {
        public DataGridView IpArray => ipArray;
        public Button GetStartButton => StartButton;
        public Button GetSetIpButton => SetLocalIpButton;
        public Status Status;
        public Chart  Chart;

        private ThreadController    _threadController;
        private InternetAdress      _internetAdress;


        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод который запускается во время загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            InitComponennt();


            tabControl1.TabPages[0].Text = "Главная";
            tabControl1.TabPages[1].Text = "Status";
            tabControl1.TabPages[2].Text = "График";

            InitIpComboBox();
           
            InternetIpList.SelectedItem = InternetIpList.Items[0];



        }
       
        
        private void InitComponennt()
        {
            Status = new Status();
            _threadController = new ThreadController(Status, this);
            _internetAdress = new InternetAdress();
            Chart = new Chart(cartesianChart, Status);
        }

        /// <summary>
        /// Метод инциализации Combox
        /// </summary>
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
            Status.IpList = InternetIpList.SelectedItem.ToString();
            foreach (var item in testListBox.Items)
            {
                Status.IpList = item.ToString();
            }
           
            _threadController.StartThread();


        }
        /// <summary>
        /// Смена активного состоянии для кнопки старт и стоп
        /// </summary>
        public void ChangeStateControllsButtons()
        {
            StopButton.Enabled = !StopButton.Enabled;
            Thread.Sleep(1000);
            StartButton.Enabled = !StartButton.Enabled;
        }

        /// <summary>
        /// Обновление сводки во вкладке статус
        /// </summary>
        public void UpdateStatusSummary()
        {
            AvgPingTextBox.Text = Status.AvgPing.ToString();
            MaxPingTextBox.Text = Status.MaxPing.ToString();
            NetworkStatusTextBox.Text = Status.NetworkStatus.ToString();
            CountMembersTextBox.Text = Status.CountMembers.ToString();
            intenetStatusTextBox.Text = Status.IntenetStatus.ToString();
        }
        /// <summary>
        /// Срабатывает при закрытии формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosing(object sender, EventArgs e)
        {
            _threadController.CloseThread();
            Environment.Exit(0);
        }

        //private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start(botLink.Text);
        //}

        private void StopButton_Click(object sender, EventArgs e)
        {
            _threadController.CloseThread();
            ChangeStateControllsButtons();
        }

        private void configLabel_Click(object sender, EventArgs e)
        {
            Process.Start("nmon.json");
        }

        private void logLabel_Click(object sender, EventArgs e)
        {
            Process.Start("log.txt");
        }

        private void SetLocalIpButton_Click(object sender, EventArgs e)
        {
            LocalNetwork.SetIpList(this,
                _internetAdress.GetIp(InternetIpList.SelectedItem.ToString()),SubnetMask.Text);
            SetLocalIpButton.Enabled = false;
            StartButton.Enabled = false;
        }

      

        private void SubnetMask_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (Char.IsDigit(ch) || ch == 8 || ch == 46)
                return;
            else
                e.Handled = true;
        }
    }
}
