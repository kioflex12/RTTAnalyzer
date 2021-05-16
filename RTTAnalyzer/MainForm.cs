using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace RTTAnalyser
{
    public partial class MainForm : Form
    {
        public DataGridView IpArray => ipArray;
        
        public Status Status;
        public Chart  Chart;

        private DirectoryController _directoryController;
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
            _directoryController = new DirectoryController();
            Status = new Status();
            _threadController = new ThreadController(Status, this);
            _internetAdress = new InternetAdress();
            Chart = new Chart(cartesianChart, Status);
            _directoryController.ClearTempDirectory();


            tabControl1.TabPages[0].Text = "Главная";
            tabControl1.TabPages[1].Text = "Status";
            tabControl1.TabPages[2].Text = "График";

            InitIpComboBox();
            InternetIpList.SelectedItem = InternetIpList.Items[0];



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
            LocalNetwork.SetIpList(Status,
                _internetAdress.GetIp(InternetIpList.SelectedItem.ToString()));
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
    }
}
