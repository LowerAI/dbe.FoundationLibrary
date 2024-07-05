namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class HCISettingsDlg1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HCISettingsDlg1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gp_Slave = new System.Windows.Forms.GroupBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.btn_Scan = new System.Windows.Forms.Button();
            this.lbl_ScanStatus = new System.Windows.Forms.Label();
            this.dgv_SlaveDevices = new System.Windows.Forms.DataGridView();
            this.dgvc_SlaveSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvc_SlaveFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvc_BtnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gp_Host = new System.Windows.Forms.GroupBox();
            this.tlp_Host = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_ExternalUARTModule = new System.Windows.Forms.Panel();
            this.lbl_PortStatus = new System.Windows.Forms.Label();
            this.tglbtn_Switch = new dbe.FoundationLibrary.Windows.UI.CustomControls.ToggleButton();
            this.cbb_BaudRates = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_Ports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.pnl_InnerBluetoothModule = new System.Windows.Forms.Panel();
            this.dgv_HostDevices = new System.Windows.Forms.DataGridView();
            this.dgvc_HostSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvc_HostMac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radbtn_IndependentBluetoothModule = new System.Windows.Forms.RadioButton();
            this.radbtn_UARTBluetoothModule = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gp_Slave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaveDevices)).BeginInit();
            this.gp_Host.SuspendLayout();
            this.tlp_Host.SuspendLayout();
            this.pnl_ExternalUARTModule.SuspendLayout();
            this.pnl_InnerBluetoothModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HostDevices)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_Slave
            // 
            this.gp_Slave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gp_Slave.Controls.Add(this.lbl_Info);
            this.gp_Slave.Controls.Add(this.btn_Scan);
            this.gp_Slave.Controls.Add(this.lbl_ScanStatus);
            this.gp_Slave.Controls.Add(this.dgv_SlaveDevices);
            this.gp_Slave.Location = new System.Drawing.Point(8, 243);
            this.gp_Slave.Name = "gp_Slave";
            this.gp_Slave.Size = new System.Drawing.Size(401, 260);
            this.gp_Slave.TabIndex = 26;
            this.gp_Slave.TabStop = false;
            this.gp_Slave.Text = "从机";
            // 
            // lbl_Info
            // 
            this.lbl_Info.BackColor = System.Drawing.SystemColors.Info;
            this.lbl_Info.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Info.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbl_Info.ForeColor = System.Drawing.Color.Gray;
            this.lbl_Info.Location = new System.Drawing.Point(9, 24);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(385, 20);
            this.lbl_Info.TabIndex = 22;
            this.lbl_Info.Text = "请双击非空行的行头来切换设备";
            this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Scan
            // 
            this.btn_Scan.AutoSize = true;
            this.btn_Scan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Scan.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_Scan.Location = new System.Drawing.Point(344, 53);
            this.btn_Scan.Name = "btn_Scan";
            this.btn_Scan.Size = new System.Drawing.Size(52, 30);
            this.btn_Scan.TabIndex = 21;
            this.btn_Scan.Text = "扫描";
            this.btn_Scan.UseVisualStyleBackColor = true;
            this.btn_Scan.Click += new System.EventHandler(this.btn_Scan_Click);
            // 
            // lbl_ScanStatus
            // 
            this.lbl_ScanStatus.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_ScanStatus.Location = new System.Drawing.Point(9, 50);
            this.lbl_ScanStatus.Name = "lbl_ScanStatus";
            this.lbl_ScanStatus.Size = new System.Drawing.Size(329, 32);
            this.lbl_ScanStatus.TabIndex = 12;
            this.lbl_ScanStatus.Text = "   ";
            // 
            // dgv_SlaveDevices
            // 
            this.dgv_SlaveDevices.AllowUserToResizeColumns = false;
            this.dgv_SlaveDevices.AllowUserToResizeRows = false;
            this.dgv_SlaveDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_SlaveDevices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv_SlaveDevices.BackgroundColor = System.Drawing.Color.Silver;
            this.dgv_SlaveDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_SlaveDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_SlaveDevices.ColumnHeadersHeight = 30;
            this.dgv_SlaveDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_SlaveDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvc_SlaveSN,
            this.dgvc_SlaveFrom,
            this.dgvc_BtnDelete});
            this.dgv_SlaveDevices.EnableHeadersVisualStyles = false;
            this.dgv_SlaveDevices.Location = new System.Drawing.Point(3, 88);
            this.dgv_SlaveDevices.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_SlaveDevices.MultiSelect = false;
            this.dgv_SlaveDevices.Name = "dgv_SlaveDevices";
            this.dgv_SlaveDevices.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_SlaveDevices.RowHeadersWidth = 25;
            this.dgv_SlaveDevices.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv_SlaveDevices.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_SlaveDevices.RowTemplate.Height = 30;
            this.dgv_SlaveDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_SlaveDevices.Size = new System.Drawing.Size(395, 166);
            this.dgv_SlaveDevices.TabIndex = 11;
            this.dgv_SlaveDevices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaveDevices_CellContentClick);
            this.dgv_SlaveDevices.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_SlaveDevices_CellEndEdit);
            this.dgv_SlaveDevices.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_SlaveDevices_RowHeaderMouseDoubleClick);
            this.dgv_SlaveDevices.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_SlaveDevices_RowPostPaint);
            this.dgv_SlaveDevices.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgv_SlaveDevices_RowsAdded);
            // 
            // dgvc_SlaveSN
            // 
            this.dgvc_SlaveSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvc_SlaveSN.DataPropertyName = "SN";
            this.dgvc_SlaveSN.FillWeight = 32F;
            this.dgvc_SlaveSN.HeaderText = "设备名";
            this.dgvc_SlaveSN.MinimumWidth = 60;
            this.dgvc_SlaveSN.Name = "dgvc_SlaveSN";
            // 
            // dgvc_SlaveFrom
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F);
            this.dgvc_SlaveFrom.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvc_SlaveFrom.HeaderText = "来源";
            this.dgvc_SlaveFrom.MinimumWidth = 74;
            this.dgvc_SlaveFrom.Name = "dgvc_SlaveFrom";
            this.dgvc_SlaveFrom.Width = 74;
            // 
            // dgvc_BtnDelete
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.dgvc_BtnDelete.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvc_BtnDelete.FillWeight = 8F;
            this.dgvc_BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.dgvc_BtnDelete.HeaderText = "删";
            this.dgvc_BtnDelete.MinimumWidth = 36;
            this.dgvc_BtnDelete.Name = "dgvc_BtnDelete";
            this.dgvc_BtnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvc_BtnDelete.Text = "✕";
            this.dgvc_BtnDelete.ToolTipText = "点击删除本行";
            this.dgvc_BtnDelete.Width = 36;
            // 
            // gp_Host
            // 
            this.gp_Host.Controls.Add(this.tlp_Host);
            this.gp_Host.Controls.Add(this.radbtn_IndependentBluetoothModule);
            this.gp_Host.Controls.Add(this.radbtn_UARTBluetoothModule);
            this.gp_Host.Location = new System.Drawing.Point(8, 27);
            this.gp_Host.Margin = new System.Windows.Forms.Padding(2);
            this.gp_Host.Name = "gp_Host";
            this.gp_Host.Padding = new System.Windows.Forms.Padding(0);
            this.gp_Host.Size = new System.Drawing.Size(401, 199);
            this.gp_Host.TabIndex = 27;
            this.gp_Host.TabStop = false;
            this.gp_Host.Text = "主机";
            // 
            // tlp_Host
            // 
            this.tlp_Host.ColumnCount = 1;
            this.tlp_Host.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Host.Controls.Add(this.pnl_ExternalUARTModule, 0, 0);
            this.tlp_Host.Controls.Add(this.pnl_InnerBluetoothModule, 0, 1);
            this.tlp_Host.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlp_Host.Location = new System.Drawing.Point(0, 52);
            this.tlp_Host.Name = "tlp_Host";
            this.tlp_Host.RowCount = 2;
            this.tlp_Host.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Host.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tlp_Host.Size = new System.Drawing.Size(401, 147);
            this.tlp_Host.TabIndex = 2;
            // 
            // pnl_ExternalUARTModule
            // 
            this.pnl_ExternalUARTModule.Controls.Add(this.lbl_PortStatus);
            this.pnl_ExternalUARTModule.Controls.Add(this.tglbtn_Switch);
            this.pnl_ExternalUARTModule.Controls.Add(this.cbb_BaudRates);
            this.pnl_ExternalUARTModule.Controls.Add(this.label2);
            this.pnl_ExternalUARTModule.Controls.Add(this.cbb_Ports);
            this.pnl_ExternalUARTModule.Controls.Add(this.label1);
            this.pnl_ExternalUARTModule.Controls.Add(this.btn_Refresh);
            this.pnl_ExternalUARTModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ExternalUARTModule.Location = new System.Drawing.Point(3, 3);
            this.pnl_ExternalUARTModule.Name = "pnl_ExternalUARTModule";
            this.pnl_ExternalUARTModule.Size = new System.Drawing.Size(395, 141);
            this.pnl_ExternalUARTModule.TabIndex = 0;
            // 
            // lbl_PortStatus
            // 
            this.lbl_PortStatus.AutoSize = true;
            this.lbl_PortStatus.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_PortStatus.Location = new System.Drawing.Point(5, 77);
            this.lbl_PortStatus.Name = "lbl_PortStatus";
            this.lbl_PortStatus.Size = new System.Drawing.Size(35, 17);
            this.lbl_PortStatus.TabIndex = 38;
            this.lbl_PortStatus.Text = "   ";
            // 
            // tglbtn_Switch
            // 
            this.tglbtn_Switch.Appearance = System.Windows.Forms.Appearance.Button;
            this.tglbtn_Switch.Location = new System.Drawing.Point(255, 69);
            this.tglbtn_Switch.MinimumSize = new System.Drawing.Size(45, 22);
            this.tglbtn_Switch.Name = "tglbtn_Switch";
            this.tglbtn_Switch.OnBackColor = System.Drawing.Color.Green;
            this.tglbtn_Switch.Size = new System.Drawing.Size(134, 70);
            this.tglbtn_Switch.SolidStyle = true;
            this.tglbtn_Switch.TabIndex = 36;
            this.tglbtn_Switch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tglbtn_Switch.UseVisualStyleBackColor = true;
            this.tglbtn_Switch.Click += new System.EventHandler(this.tglbtn_Switch_Click);
            // 
            // cbb_BaudRates
            // 
            this.cbb_BaudRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_BaudRates.Font = new System.Drawing.Font("宋体", 11F);
            this.cbb_BaudRates.FormattingEnabled = true;
            this.cbb_BaudRates.Location = new System.Drawing.Point(93, 113);
            this.cbb_BaudRates.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_BaudRates.Name = "cbb_BaudRates";
            this.cbb_BaudRates.Size = new System.Drawing.Size(136, 26);
            this.cbb_BaudRates.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(1, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 34;
            this.label2.Text = "波特率：";
            // 
            // cbb_Ports
            // 
            this.cbb_Ports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Ports.Font = new System.Drawing.Font("宋体", 10F);
            this.cbb_Ports.FormattingEnabled = true;
            this.cbb_Ports.Location = new System.Drawing.Point(1, 34);
            this.cbb_Ports.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_Ports.Name = "cbb_Ports";
            this.cbb_Ports.Size = new System.Drawing.Size(390, 25);
            this.cbb_Ports.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(1, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 32;
            this.label1.Text = "串口选择：";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.btn_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_Refresh.Image")));
            this.btn_Refresh.Location = new System.Drawing.Point(357, 0);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(35, 35);
            this.btn_Refresh.TabIndex = 37;
            this.btn_Refresh.UseVisualStyleBackColor = false;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // pnl_InnerBluetoothModule
            // 
            this.pnl_InnerBluetoothModule.Controls.Add(this.dgv_HostDevices);
            this.pnl_InnerBluetoothModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_InnerBluetoothModule.Location = new System.Drawing.Point(3, 150);
            this.pnl_InnerBluetoothModule.Name = "pnl_InnerBluetoothModule";
            this.pnl_InnerBluetoothModule.Size = new System.Drawing.Size(395, 1);
            this.pnl_InnerBluetoothModule.TabIndex = 1;
            // 
            // dgv_HostDevices
            // 
            this.dgv_HostDevices.AllowUserToAddRows = false;
            this.dgv_HostDevices.AllowUserToResizeColumns = false;
            this.dgv_HostDevices.AllowUserToResizeRows = false;
            this.dgv_HostDevices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dgv_HostDevices.BackgroundColor = System.Drawing.Color.Silver;
            this.dgv_HostDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_HostDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_HostDevices.ColumnHeadersHeight = 30;
            this.dgv_HostDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_HostDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvc_HostSN,
            this.dgvc_HostMac});
            this.dgv_HostDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_HostDevices.EnableHeadersVisualStyles = false;
            this.dgv_HostDevices.Location = new System.Drawing.Point(0, 0);
            this.dgv_HostDevices.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_HostDevices.MultiSelect = false;
            this.dgv_HostDevices.Name = "dgv_HostDevices";
            this.dgv_HostDevices.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_HostDevices.RowHeadersWidth = 25;
            this.dgv_HostDevices.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv_HostDevices.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_HostDevices.RowTemplate.Height = 30;
            this.dgv_HostDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_HostDevices.Size = new System.Drawing.Size(395, 1);
            this.dgv_HostDevices.TabIndex = 12;
            // 
            // dgvc_HostSN
            // 
            this.dgvc_HostSN.DataPropertyName = "SN";
            this.dgvc_HostSN.HeaderText = "设备名";
            this.dgvc_HostSN.MinimumWidth = 60;
            this.dgvc_HostSN.Name = "dgvc_HostSN";
            this.dgvc_HostSN.Width = 200;
            // 
            // dgvc_HostMac
            // 
            this.dgvc_HostMac.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F);
            this.dgvc_HostMac.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvc_HostMac.HeaderText = "MAC地址";
            this.dgvc_HostMac.MinimumWidth = 74;
            this.dgvc_HostMac.Name = "dgvc_HostMac";
            // 
            // radbtn_IndependentBluetoothModule
            // 
            this.radbtn_IndependentBluetoothModule.AutoSize = true;
            this.radbtn_IndependentBluetoothModule.Location = new System.Drawing.Point(245, 25);
            this.radbtn_IndependentBluetoothModule.Name = "radbtn_IndependentBluetoothModule";
            this.radbtn_IndependentBluetoothModule.Size = new System.Drawing.Size(144, 23);
            this.radbtn_IndependentBluetoothModule.TabIndex = 1;
            this.radbtn_IndependentBluetoothModule.Text = "独立蓝牙模块";
            this.radbtn_IndependentBluetoothModule.UseVisualStyleBackColor = true;
            this.radbtn_IndependentBluetoothModule.CheckedChanged += new System.EventHandler(this.radbtn_BluetoothModule_CheckedChanged);
            // 
            // radbtn_UARTBluetoothModule
            // 
            this.radbtn_UARTBluetoothModule.AutoSize = true;
            this.radbtn_UARTBluetoothModule.Checked = true;
            this.radbtn_UARTBluetoothModule.Location = new System.Drawing.Point(9, 25);
            this.radbtn_UARTBluetoothModule.Name = "radbtn_UARTBluetoothModule";
            this.radbtn_UARTBluetoothModule.Size = new System.Drawing.Size(146, 23);
            this.radbtn_UARTBluetoothModule.TabIndex = 0;
            this.radbtn_UARTBluetoothModule.TabStop = true;
            this.radbtn_UARTBluetoothModule.Text = "UART蓝牙模块";
            this.radbtn_UARTBluetoothModule.UseVisualStyleBackColor = true;
            this.radbtn_UARTBluetoothModule.CheckedChanged += new System.EventHandler(this.radbtn_BluetoothModule_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(8, 234);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 270);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "从机";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Info;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(9, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(385, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "请双击非空行的行头来切换设备";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("宋体", 10F);
            this.button1.Location = new System.Drawing.Point(344, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 30);
            this.button1.TabIndex = 21;
            this.button1.Text = "扫描";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_Scan_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 10F);
            this.label4.Location = new System.Drawing.Point(9, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(329, 32);
            this.label4.TabIndex = 12;
            this.label4.Text = "   ";
            // 
            // HCISettingsDlg1
            // 
            this.ClientSize = new System.Drawing.Size(416, 506);
            this.Controls.Add(this.gp_Host);
            this.Controls.Add(this.gp_Slave);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Name = "HCISettingsDlg1";
            this.Text = "HCI设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortSettingsDlg_FormClosing);
            this.Load += new System.EventHandler(this.SerialPortSettingsDlg_Load);
            this.Controls.SetChildIndex(this.gp_Slave, 0);
            this.Controls.SetChildIndex(this.gp_Host, 0);
            this.gp_Slave.ResumeLayout(false);
            this.gp_Slave.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaveDevices)).EndInit();
            this.gp_Host.ResumeLayout(false);
            this.gp_Host.PerformLayout();
            this.tlp_Host.ResumeLayout(false);
            this.pnl_ExternalUARTModule.ResumeLayout(false);
            this.pnl_ExternalUARTModule.PerformLayout();
            this.pnl_InnerBluetoothModule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HostDevices)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gp_Slave;
        private System.Windows.Forms.Label lbl_ScanStatus;
        private System.Windows.Forms.DataGridView dgv_SlaveDevices;
        private System.Windows.Forms.Button btn_Scan;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.GroupBox gp_Host;
        private System.Windows.Forms.RadioButton radbtn_IndependentBluetoothModule;
        private System.Windows.Forms.RadioButton radbtn_UARTBluetoothModule;
        private System.Windows.Forms.TableLayoutPanel tlp_Host;
        private System.Windows.Forms.Panel pnl_ExternalUARTModule;
        private System.Windows.Forms.Panel pnl_InnerBluetoothModule;
        private System.Windows.Forms.Label lbl_PortStatus;
        private CustomControls.ToggleButton tglbtn_Switch;
        private System.Windows.Forms.ComboBox cbb_BaudRates;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbb_Ports;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgv_HostDevices;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_SlaveSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_SlaveFrom;
        private System.Windows.Forms.DataGridViewButtonColumn dgvc_BtnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_HostSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_HostMac;
    }
}