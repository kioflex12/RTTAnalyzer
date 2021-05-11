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
        private List<string> ipList = new List<string>() { "1.1.1.1", "8.8.8.8" };

        public MainForm()
        {
            InitializeComponent();
        }

        public Label GetStatusLabel => StatusLabel;
        public DataGridView IpArray => ipArray;

        private void StartButton_Click(object sender, EventArgs e)
        {

            Thread task = new Thread(StartAscync);
            task.Start();

        }

        private void StartAscync()
        {
            Unalyzer.InitAnylyzer(ipList, this);
        }

        private void StartButton_Click_1(object sender, EventArgs e)
        {

        }
    }
}
