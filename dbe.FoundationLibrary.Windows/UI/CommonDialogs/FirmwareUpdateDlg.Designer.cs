namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class FirmwareUpdateDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_NewFirmware = new System.Windows.Forms.Label();
            this.lbl_CurrentFirmware = new System.Windows.Forms.Label();
            this.btn_Update = new System.Windows.Forms.Button();
            this.lbl_Crc32 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_ReloadSN = new System.Windows.Forms.Label();
            this.tt_tip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(9, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 29;
            this.label2.Text = "新固件：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(9, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 20);
            this.label1.TabIndex = 28;
            this.label1.Text = "当前固件版本：";
            // 
            // lbl_NewFirmware
            // 
            this.lbl_NewFirmware.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_NewFirmware.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbl_NewFirmware.Location = new System.Drawing.Point(96, 83);
            this.lbl_NewFirmware.Name = "lbl_NewFirmware";
            this.lbl_NewFirmware.Size = new System.Drawing.Size(168, 24);
            this.lbl_NewFirmware.TabIndex = 31;
            this.lbl_NewFirmware.Text = "未读取";
            this.lbl_NewFirmware.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tt_tip.SetToolTip(this.lbl_NewFirmware, "双击加载新固件");
            this.lbl_NewFirmware.DoubleClick += new System.EventHandler(this.lbl_NewFirmware_DoubleClick);
            // 
            // lbl_CurrentFirmware
            // 
            this.lbl_CurrentFirmware.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_CurrentFirmware.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbl_CurrentFirmware.Location = new System.Drawing.Point(144, 48);
            this.lbl_CurrentFirmware.Name = "lbl_CurrentFirmware";
            this.lbl_CurrentFirmware.Size = new System.Drawing.Size(112, 24);
            this.lbl_CurrentFirmware.TabIndex = 30;
            this.lbl_CurrentFirmware.Text = "未校准";
            this.lbl_CurrentFirmware.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Update
            // 
            this.btn_Update.AutoSize = true;
            this.btn_Update.Enabled = false;
            this.btn_Update.Location = new System.Drawing.Point(102, 144);
            this.btn_Update.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(76, 37);
            this.btn_Update.TabIndex = 32;
            this.btn_Update.Text = "更新";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // lbl_Crc32
            // 
            this.lbl_Crc32.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Crc32.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbl_Crc32.Location = new System.Drawing.Point(96, 118);
            this.lbl_Crc32.Name = "lbl_Crc32";
            this.lbl_Crc32.Size = new System.Drawing.Size(168, 24);
            this.lbl_Crc32.TabIndex = 34;
            this.lbl_Crc32.Text = "未读取";
            this.lbl_Crc32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(9, 120);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 33;
            this.label4.Text = "Crc32：";
            // 
            // lbl_ReloadSN
            // 
            this.lbl_ReloadSN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_ReloadSN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbl_ReloadSN.Location = new System.Drawing.Point(264, 52);
            this.lbl_ReloadSN.Name = "lbl_ReloadSN";
            this.lbl_ReloadSN.Size = new System.Drawing.Size(22, 18);
            this.lbl_ReloadSN.TabIndex = 46;
            this.lbl_ReloadSN.Text = "↻";
            this.lbl_ReloadSN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tt_tip.SetToolTip(this.lbl_ReloadSN, "读取当前固件版本号");
            this.lbl_ReloadSN.UseCompatibleTextRendering = true;
            this.lbl_ReloadSN.Click += new System.EventHandler(this.lbl_ReloadSN_Click);
            // 
            // FirmwareUpdateDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 188);
            this.Controls.Add(this.lbl_ReloadSN);
            this.Controls.Add(this.lbl_Crc32);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.lbl_NewFirmware);
            this.Controls.Add(this.lbl_CurrentFirmware);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FirmwareUpdateDlg";
            this.Text = "固件更新";
            this.Load += new System.EventHandler(this.FirmwareUpdateDlg_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lbl_CurrentFirmware, 0);
            this.Controls.SetChildIndex(this.lbl_NewFirmware, 0);
            this.Controls.SetChildIndex(this.btn_Update, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.lbl_Crc32, 0);
            this.Controls.SetChildIndex(this.lbl_ReloadSN, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_NewFirmware;
        private System.Windows.Forms.Label lbl_CurrentFirmware;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.Label lbl_Crc32;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_ReloadSN;
        private System.Windows.Forms.ToolTip tt_tip;
    }
}