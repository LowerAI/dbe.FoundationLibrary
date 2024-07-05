namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class SerialPortSettingsDlg
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialPortSettingsDlg));
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.tglbtn_Switch = new dbe.FoundationLibrary.Windows.UI.CustomControls.ToggleButton();
            this.cbb_BaudRates = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_Ports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.btn_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_Refresh.Image")));
            this.btn_Refresh.Location = new System.Drawing.Point(363, 31);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(35, 35);
            this.btn_Refresh.TabIndex = 24;
            this.btn_Refresh.UseVisualStyleBackColor = false;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // tglbtn_Switch
            // 
            this.tglbtn_Switch.Location = new System.Drawing.Point(261, 103);
            this.tglbtn_Switch.MinimumSize = new System.Drawing.Size(45, 22);
            this.tglbtn_Switch.Name = "tglbtn_Switch";
            this.tglbtn_Switch.OnBackColor = System.Drawing.Color.Green;
            this.tglbtn_Switch.Size = new System.Drawing.Size(134, 70);
            this.tglbtn_Switch.SolidStyle = true;
            this.tglbtn_Switch.TabIndex = 23;
            this.tglbtn_Switch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tglbtn_Switch.UseVisualStyleBackColor = true;
            this.tglbtn_Switch.Click += new System.EventHandler(this.tglbtn_Switch_Click);
            // 
            // cbb_BaudRates
            // 
            this.cbb_BaudRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_BaudRates.Font = new System.Drawing.Font("宋体", 11F);
            this.cbb_BaudRates.FormattingEnabled = true;
            this.cbb_BaudRates.Location = new System.Drawing.Point(99, 147);
            this.cbb_BaudRates.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_BaudRates.Name = "cbb_BaudRates";
            this.cbb_BaudRates.Size = new System.Drawing.Size(136, 26);
            this.cbb_BaudRates.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(7, 150);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "波特率：";
            // 
            // cbb_Ports
            // 
            this.cbb_Ports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Ports.Font = new System.Drawing.Font("宋体", 10F);
            this.cbb_Ports.FormattingEnabled = true;
            this.cbb_Ports.Location = new System.Drawing.Point(7, 65);
            this.cbb_Ports.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_Ports.Name = "cbb_Ports";
            this.cbb_Ports.Size = new System.Drawing.Size(390, 25);
            this.cbb_Ports.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "串口选择：";
            // 
            // SerialPortSettingsDlg
            // 
            this.ClientSize = new System.Drawing.Size(405, 180);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.tglbtn_Switch);
            this.Controls.Add(this.cbb_BaudRates);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbb_Ports);
            this.Controls.Add(this.label1);
            this.Name = "SerialPortSettingsDlg";
            this.Text = "串口设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortSettingsDlg_FormClosing);
            this.Load += new System.EventHandler(this.SerialPortSettingsDlg_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cbb_Ports, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbb_BaudRates, 0);
            this.Controls.SetChildIndex(this.tglbtn_Switch, 0);
            this.Controls.SetChildIndex(this.btn_Refresh, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Refresh;
        private CustomControls.ToggleButton tglbtn_Switch;
        private System.Windows.Forms.ComboBox cbb_BaudRates;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbb_Ports;
        private System.Windows.Forms.Label label1;
    }
}
