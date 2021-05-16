
namespace RTTAnalyser
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.InternetIpList = new System.Windows.Forms.ComboBox();
            this.configLabel = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.ipArray = new System.Windows.Forms.DataGridView();
            this.StartButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.intenetStatusTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CountMembersTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NetworkStatusTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MaxPingTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AvgPingTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.logLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipArray)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(422, 315);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.logLabel);
            this.tabPage1.Controls.Add(this.InternetIpList);
            this.tabPage1.Controls.Add(this.configLabel);
            this.tabPage1.Controls.Add(this.StopButton);
            this.tabPage1.Controls.Add(this.ipArray);
            this.tabPage1.Controls.Add(this.StartButton);
            this.tabPage1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(414, 289);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // InternetIpList
            // 
            this.InternetIpList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InternetIpList.FormattingEnabled = true;
            this.InternetIpList.Location = new System.Drawing.Point(287, 53);
            this.InternetIpList.Name = "InternetIpList";
            this.InternetIpList.Size = new System.Drawing.Size(121, 21);
            this.InternetIpList.TabIndex = 7;
            // 
            // configLabel
            // 
            this.configLabel.AutoSize = true;
            this.configLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.configLabel.Location = new System.Drawing.Point(342, 12);
            this.configLabel.Name = "configLabel";
            this.configLabel.Size = new System.Drawing.Size(37, 13);
            this.configLabel.TabIndex = 6;
            this.configLabel.Text = "Config";
            this.configLabel.Click += new System.EventHandler(this.configLabel_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(221, 229);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 5;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ipArray
            // 
            this.ipArray.AllowUserToAddRows = false;
            this.ipArray.AllowUserToDeleteRows = false;
            this.ipArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ipArray.Location = new System.Drawing.Point(37, 35);
            this.ipArray.Name = "ipArray";
            this.ipArray.ReadOnly = true;
            this.ipArray.Size = new System.Drawing.Size(245, 163);
            this.ipArray.TabIndex = 3;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(302, 229);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.intenetStatusTextBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.CountMembersTextBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.NetworkStatusTextBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.MaxPingTextBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.AvgPingTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(414, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "IntenetStatus";
            // 
            // intenetStatusTextBox
            // 
            this.intenetStatusTextBox.Location = new System.Drawing.Point(126, 91);
            this.intenetStatusTextBox.Name = "intenetStatusTextBox";
            this.intenetStatusTextBox.ReadOnly = true;
            this.intenetStatusTextBox.Size = new System.Drawing.Size(100, 20);
            this.intenetStatusTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "CountMembers";
            // 
            // CountMembersTextBox
            // 
            this.CountMembersTextBox.Location = new System.Drawing.Point(18, 91);
            this.CountMembersTextBox.Name = "CountMembersTextBox";
            this.CountMembersTextBox.ReadOnly = true;
            this.CountMembersTextBox.Size = new System.Drawing.Size(100, 20);
            this.CountMembersTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "NetworkStatus";
            // 
            // NetworkStatusTextBox
            // 
            this.NetworkStatusTextBox.Location = new System.Drawing.Point(230, 32);
            this.NetworkStatusTextBox.Name = "NetworkStatusTextBox";
            this.NetworkStatusTextBox.ReadOnly = true;
            this.NetworkStatusTextBox.Size = new System.Drawing.Size(100, 20);
            this.NetworkStatusTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MaxPing";
            // 
            // MaxPingTextBox
            // 
            this.MaxPingTextBox.Location = new System.Drawing.Point(124, 32);
            this.MaxPingTextBox.Name = "MaxPingTextBox";
            this.MaxPingTextBox.ReadOnly = true;
            this.MaxPingTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxPingTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "AvgPing";
            // 
            // AvgPingTextBox
            // 
            this.AvgPingTextBox.Location = new System.Drawing.Point(18, 32);
            this.AvgPingTextBox.Name = "AvgPingTextBox";
            this.AvgPingTextBox.ReadOnly = true;
            this.AvgPingTextBox.Size = new System.Drawing.Size(100, 20);
            this.AvgPingTextBox.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cartesianChart);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(414, 289);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cartesianChart
            // 
            this.cartesianChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart.Location = new System.Drawing.Point(3, 3);
            this.cartesianChart.Name = "cartesianChart";
            this.cartesianChart.Size = new System.Drawing.Size(408, 283);
            this.cartesianChart.TabIndex = 0;
            this.cartesianChart.Text = "cartesianChart";
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logLabel.Location = new System.Drawing.Point(34, 239);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(25, 13);
            this.logLabel.TabIndex = 8;
            this.logLabel.Text = "Log";
            this.logLabel.Click += new System.EventHandler(this.logLabel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 315);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipArray)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView ipArray;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CountMembersTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NetworkStatusTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MaxPingTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AvgPingTextBox;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Label configLabel;
        private System.Windows.Forms.ComboBox InternetIpList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox intenetStatusTextBox;
        private System.Windows.Forms.TabPage tabPage3;
        public LiveCharts.WinForms.CartesianChart cartesianChart;
        private System.Windows.Forms.Label logLabel;
    }
}

