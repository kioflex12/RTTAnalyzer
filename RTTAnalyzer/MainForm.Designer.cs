
namespace RTTAnalyzer
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
            this.ipArray = new System.Windows.Forms.DataGridView();
            this.IpAvgPing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ipArray)).BeginInit();
            this.SuspendLayout();
            // 
            // ipArray
            // 
            this.ipArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ipArray.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IpAvgPing});
            this.ipArray.Location = new System.Drawing.Point(96, 95);
            this.ipArray.Name = "ipArray";
            this.ipArray.Size = new System.Drawing.Size(240, 150);
            this.ipArray.TabIndex = 0;
            // 
            // IpAvgPing
            // 
            this.IpAvgPing.HeaderText = "IpAvgPing";
            this.IpAvgPing.Name = "IpAvgPing";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(451, 175);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "button1";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(479, 95);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(35, 13);
            this.StatusLabel.TabIndex = 2;
            this.StatusLabel.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ipArray);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ipArray)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ipArray;
        private System.Windows.Forms.DataGridViewTextBoxColumn IpAvgPing;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label StatusLabel;
    }
}

