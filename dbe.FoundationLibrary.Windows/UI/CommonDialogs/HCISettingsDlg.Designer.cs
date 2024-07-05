namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class HCISettingsDlg
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HCISettingsDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlp_Global = new System.Windows.Forms.TableLayoutPanel();
            this.gp_Host = new System.Windows.Forms.GroupBox();
            this.gp_CommunicationMode = new System.Windows.Forms.GroupBox();
            this.radbtn_DirectConnection = new System.Windows.Forms.RadioButton();
            this.radbtn_BluetoothToSerial = new System.Windows.Forms.RadioButton();
            this.radbtn_ESBToSerial = new System.Windows.Forms.RadioButton();
            this.gp_SerialConnectionMode = new System.Windows.Forms.GroupBox();
            this.radbtn_WirelessSerialPort = new System.Windows.Forms.RadioButton();
            this.radbtn_WiredSerialPort = new System.Windows.Forms.RadioButton();
            this.lbl_PortStatus = new System.Windows.Forms.Label();
            this.tglbtn_Switch = new dbe.FoundationLibrary.Windows.UI.CustomControls.ToggleButton();
            this.cbb_BaudRates = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_Ports = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.gp_Slave0 = new System.Windows.Forms.GroupBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.btn_Scan0 = new System.Windows.Forms.Button();
            this.lbl_ScanStatus0 = new System.Windows.Forms.Label();
            this.dgv_SlaveDevices = new System.Windows.Forms.DataGridView();
            this.dgvc_TxtSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvc_TxtFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvc_BtnDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gp_Slave1 = new System.Windows.Forms.GroupBox();
            this.lbl_ScanStatus1 = new System.Windows.Forms.Label();
            this.btn_Scan1 = new System.Windows.Forms.Button();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.nud_CurrentCommunicationChannel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tt_tip = new System.Windows.Forms.ToolTip(this.components);
            this.tlp_Global.SuspendLayout();
            this.gp_Host.SuspendLayout();
            this.gp_CommunicationMode.SuspendLayout();
            this.gp_SerialConnectionMode.SuspendLayout();
            this.gp_Slave0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaveDevices)).BeginInit();
            this.gp_Slave1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CurrentCommunicationChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // tlp_Global
            // 
            this.tlp_Global.ColumnCount = 1;
            this.tlp_Global.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Global.Controls.Add(this.gp_Host, 0, 0);
            this.tlp_Global.Controls.Add(this.gp_Slave0, 0, 1);
            this.tlp_Global.Controls.Add(this.gp_Slave1, 0, 2);
            this.tlp_Global.Location = new System.Drawing.Point(1, 25);
            this.tlp_Global.Name = "tlp_Global";
            this.tlp_Global.RowCount = 3;
            this.tlp_Global.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tlp_Global.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tlp_Global.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Global.Size = new System.Drawing.Size(406, 655);
            this.tlp_Global.TabIndex = 27;
            // 
            // gp_Host
            // 
            this.gp_Host.Controls.Add(this.gp_CommunicationMode);
            this.gp_Host.Controls.Add(this.gp_SerialConnectionMode);
            this.gp_Host.Controls.Add(this.lbl_PortStatus);
            this.gp_Host.Controls.Add(this.tglbtn_Switch);
            this.gp_Host.Controls.Add(this.cbb_BaudRates);
            this.gp_Host.Controls.Add(this.label2);
            this.gp_Host.Controls.Add(this.cbb_Ports);
            this.gp_Host.Controls.Add(this.label1);
            this.gp_Host.Controls.Add(this.btn_Refresh);
            this.gp_Host.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_Host.Location = new System.Drawing.Point(3, 3);
            this.gp_Host.Name = "gp_Host";
            this.gp_Host.Size = new System.Drawing.Size(400, 208);
            this.gp_Host.TabIndex = 26;
            this.gp_Host.TabStop = false;
            this.gp_Host.Text = "主机";
            // 
            // gp_CommunicationMode
            // 
            this.gp_CommunicationMode.Controls.Add(this.radbtn_DirectConnection);
            this.gp_CommunicationMode.Controls.Add(this.radbtn_BluetoothToSerial);
            this.gp_CommunicationMode.Controls.Add(this.radbtn_ESBToSerial);
            this.gp_CommunicationMode.Enabled = false;
            this.gp_CommunicationMode.Font = new System.Drawing.Font("宋体", 10F);
            this.gp_CommunicationMode.Location = new System.Drawing.Point(5, 160);
            this.gp_CommunicationMode.Name = "gp_CommunicationMode";
            this.gp_CommunicationMode.Size = new System.Drawing.Size(387, 44);
            this.gp_CommunicationMode.TabIndex = 45;
            this.gp_CommunicationMode.TabStop = false;
            this.gp_CommunicationMode.Text = "通信方式";
            // 
            // radbtn_DirectConnection
            // 
            this.radbtn_DirectConnection.AutoSize = true;
            this.radbtn_DirectConnection.Location = new System.Drawing.Point(253, 20);
            this.radbtn_DirectConnection.Name = "radbtn_DirectConnection";
            this.radbtn_DirectConnection.Size = new System.Drawing.Size(97, 21);
            this.radbtn_DirectConnection.TabIndex = 3;
            this.radbtn_DirectConnection.Text = "串口直连";
            this.tt_tip.SetToolTip(this.radbtn_DirectConnection, "2.4G ESB转串口");
            // 
            // radbtn_BluetoothToSerial
            // 
            this.radbtn_BluetoothToSerial.AutoSize = true;
            this.radbtn_BluetoothToSerial.Location = new System.Drawing.Point(26, 20);
            this.radbtn_BluetoothToSerial.Name = "radbtn_BluetoothToSerial";
            this.radbtn_BluetoothToSerial.Size = new System.Drawing.Size(114, 21);
            this.radbtn_BluetoothToSerial.TabIndex = 1;
            this.radbtn_BluetoothToSerial.Text = "蓝牙转串口";
            // 
            // radbtn_ESBToSerial
            // 
            this.radbtn_ESBToSerial.AutoSize = true;
            this.radbtn_ESBToSerial.Location = new System.Drawing.Point(143, 20);
            this.radbtn_ESBToSerial.Name = "radbtn_ESBToSerial";
            this.radbtn_ESBToSerial.Size = new System.Drawing.Size(107, 21);
            this.radbtn_ESBToSerial.TabIndex = 2;
            this.radbtn_ESBToSerial.Text = "ESB转串口";
            this.tt_tip.SetToolTip(this.radbtn_ESBToSerial, "2.4G ESB转串口");
            // 
            // gp_SerialConnectionMode
            // 
            this.gp_SerialConnectionMode.Controls.Add(this.radbtn_WirelessSerialPort);
            this.gp_SerialConnectionMode.Controls.Add(this.radbtn_WiredSerialPort);
            this.gp_SerialConnectionMode.Enabled = false;
            this.gp_SerialConnectionMode.Font = new System.Drawing.Font("宋体", 10F);
            this.gp_SerialConnectionMode.Location = new System.Drawing.Point(5, 112);
            this.gp_SerialConnectionMode.Name = "gp_SerialConnectionMode";
            this.gp_SerialConnectionMode.Size = new System.Drawing.Size(227, 42);
            this.gp_SerialConnectionMode.TabIndex = 44;
            this.gp_SerialConnectionMode.TabStop = false;
            this.gp_SerialConnectionMode.Text = "串口连接方式";
            // 
            // radbtn_WirelessSerialPort
            // 
            this.radbtn_WirelessSerialPort.AutoSize = true;
            this.radbtn_WirelessSerialPort.Enabled = false;
            this.radbtn_WirelessSerialPort.Location = new System.Drawing.Point(13, 18);
            this.radbtn_WirelessSerialPort.Name = "radbtn_WirelessSerialPort";
            this.radbtn_WirelessSerialPort.Size = new System.Drawing.Size(97, 21);
            this.radbtn_WirelessSerialPort.TabIndex = 1;
            this.radbtn_WirelessSerialPort.Text = "无线串口";
            // 
            // radbtn_WiredSerialPort
            // 
            this.radbtn_WiredSerialPort.AutoSize = true;
            this.radbtn_WiredSerialPort.Enabled = false;
            this.radbtn_WiredSerialPort.Location = new System.Drawing.Point(117, 18);
            this.radbtn_WiredSerialPort.Name = "radbtn_WiredSerialPort";
            this.radbtn_WiredSerialPort.Size = new System.Drawing.Size(97, 21);
            this.radbtn_WiredSerialPort.TabIndex = 2;
            this.radbtn_WiredSerialPort.Text = "有线串口";
            // 
            // lbl_PortStatus
            // 
            this.lbl_PortStatus.AutoSize = true;
            this.lbl_PortStatus.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_PortStatus.Location = new System.Drawing.Point(200, 24);
            this.lbl_PortStatus.Name = "lbl_PortStatus";
            this.lbl_PortStatus.Size = new System.Drawing.Size(35, 17);
            this.lbl_PortStatus.TabIndex = 31;
            this.lbl_PortStatus.Text = "   ";
            // 
            // tglbtn_Switch
            // 
            this.tglbtn_Switch.Appearance = System.Windows.Forms.Appearance.Button;
            this.tglbtn_Switch.Location = new System.Drawing.Point(259, 84);
            this.tglbtn_Switch.MinimumSize = new System.Drawing.Size(45, 22);
            this.tglbtn_Switch.Name = "tglbtn_Switch";
            this.tglbtn_Switch.OnBackColor = System.Drawing.Color.Green;
            this.tglbtn_Switch.Size = new System.Drawing.Size(134, 70);
            this.tglbtn_Switch.SolidStyle = true;
            this.tglbtn_Switch.TabIndex = 29;
            this.tglbtn_Switch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tglbtn_Switch.UseVisualStyleBackColor = true;
            this.tglbtn_Switch.Click += new System.EventHandler(this.tglbtn_Switch_Click);
            // 
            // cbb_BaudRates
            // 
            this.cbb_BaudRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_BaudRates.Font = new System.Drawing.Font("宋体", 11F);
            this.cbb_BaudRates.FormattingEnabled = true;
            this.cbb_BaudRates.Location = new System.Drawing.Point(97, 84);
            this.cbb_BaudRates.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_BaudRates.Name = "cbb_BaudRates";
            this.cbb_BaudRates.Size = new System.Drawing.Size(136, 26);
            this.cbb_BaudRates.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(5, 87);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 27;
            this.label2.Text = "波特率：";
            // 
            // cbb_Ports
            // 
            this.cbb_Ports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_Ports.Font = new System.Drawing.Font("宋体", 10F);
            this.cbb_Ports.FormattingEnabled = true;
            this.cbb_Ports.Location = new System.Drawing.Point(5, 49);
            this.cbb_Ports.Margin = new System.Windows.Forms.Padding(2);
            this.cbb_Ports.Name = "cbb_Ports";
            this.cbb_Ports.Size = new System.Drawing.Size(390, 25);
            this.cbb_Ports.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(5, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "串口选择：";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.btn_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_Refresh.Image")));
            this.btn_Refresh.Location = new System.Drawing.Point(361, 15);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(35, 35);
            this.btn_Refresh.TabIndex = 30;
            this.btn_Refresh.UseVisualStyleBackColor = false;
            // 
            // gp_Slave0
            // 
            this.gp_Slave0.Controls.Add(this.lbl_Info);
            this.gp_Slave0.Controls.Add(this.btn_Scan0);
            this.gp_Slave0.Controls.Add(this.lbl_ScanStatus0);
            this.gp_Slave0.Controls.Add(this.dgv_SlaveDevices);
            this.gp_Slave0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_Slave0.Location = new System.Drawing.Point(3, 217);
            this.gp_Slave0.Name = "gp_Slave0";
            this.gp_Slave0.Size = new System.Drawing.Size(400, 274);
            this.gp_Slave0.TabIndex = 27;
            this.gp_Slave0.TabStop = false;
            this.gp_Slave0.Text = "从机";
            this.tt_tip.SetToolTip(this.gp_Slave0, "低功耗蓝牙转串口");
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
            // btn_Scan0
            // 
            this.btn_Scan0.AutoSize = true;
            this.btn_Scan0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Scan0.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_Scan0.Location = new System.Drawing.Point(344, 53);
            this.btn_Scan0.Name = "btn_Scan0";
            this.btn_Scan0.Size = new System.Drawing.Size(52, 30);
            this.btn_Scan0.TabIndex = 21;
            this.btn_Scan0.Text = "扫描";
            this.btn_Scan0.UseVisualStyleBackColor = true;
            this.btn_Scan0.Click += new System.EventHandler(this.btn_Scan0_Click);
            // 
            // lbl_ScanStatus0
            // 
            this.lbl_ScanStatus0.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_ScanStatus0.Location = new System.Drawing.Point(9, 51);
            this.lbl_ScanStatus0.Name = "lbl_ScanStatus0";
            this.lbl_ScanStatus0.Size = new System.Drawing.Size(329, 32);
            this.lbl_ScanStatus0.TabIndex = 12;
            this.lbl_ScanStatus0.Text = "   ";
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
            this.dgvc_TxtSN,
            this.dgvc_TxtFrom,
            this.dgvc_BtnDelete});
            this.dgv_SlaveDevices.EnableHeadersVisualStyles = false;
            this.dgv_SlaveDevices.Location = new System.Drawing.Point(3, 85);
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
            this.dgv_SlaveDevices.Size = new System.Drawing.Size(394, 186);
            this.dgv_SlaveDevices.TabIndex = 11;
            this.dgv_SlaveDevices.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_SlaveDevices_RowHeaderMouseDoubleClick);
            // 
            // dgvc_TxtSN
            // 
            this.dgvc_TxtSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvc_TxtSN.DataPropertyName = "SN";
            this.dgvc_TxtSN.FillWeight = 32F;
            this.dgvc_TxtSN.HeaderText = "设备SN";
            this.dgvc_TxtSN.MinimumWidth = 60;
            this.dgvc_TxtSN.Name = "dgvc_TxtSN";
            // 
            // dgvc_TxtFrom
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F);
            this.dgvc_TxtFrom.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvc_TxtFrom.HeaderText = "来源";
            this.dgvc_TxtFrom.MinimumWidth = 74;
            this.dgvc_TxtFrom.Name = "dgvc_TxtFrom";
            this.dgvc_TxtFrom.Width = 74;
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
            // gp_Slave1
            // 
            this.gp_Slave1.Controls.Add(this.lbl_ScanStatus1);
            this.gp_Slave1.Controls.Add(this.btn_Scan1);
            this.gp_Slave1.Controls.Add(this.btn_Modify);
            this.gp_Slave1.Controls.Add(this.nud_CurrentCommunicationChannel);
            this.gp_Slave1.Controls.Add(this.label4);
            this.gp_Slave1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_Slave1.Location = new System.Drawing.Point(3, 497);
            this.gp_Slave1.Name = "gp_Slave1";
            this.gp_Slave1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.gp_Slave1.Size = new System.Drawing.Size(400, 155);
            this.gp_Slave1.TabIndex = 28;
            this.gp_Slave1.TabStop = false;
            this.gp_Slave1.Text = "从机";
            this.tt_tip.SetToolTip(this.gp_Slave1, "2.4G ESB转串口");
            // 
            // lbl_ScanStatus1
            // 
            this.lbl_ScanStatus1.Font = new System.Drawing.Font("宋体", 10F);
            this.lbl_ScanStatus1.Location = new System.Drawing.Point(8, 120);
            this.lbl_ScanStatus1.Name = "lbl_ScanStatus1";
            this.lbl_ScanStatus1.Size = new System.Drawing.Size(384, 32);
            this.lbl_ScanStatus1.TabIndex = 35;
            this.lbl_ScanStatus1.Text = "   ";
            // 
            // btn_Scan1
            // 
            this.btn_Scan1.AutoSize = true;
            this.btn_Scan1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Scan1.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_Scan1.Location = new System.Drawing.Point(320, 63);
            this.btn_Scan1.Name = "btn_Scan1";
            this.btn_Scan1.Size = new System.Drawing.Size(52, 30);
            this.btn_Scan1.TabIndex = 33;
            this.btn_Scan1.Text = "扫描";
            this.btn_Scan1.UseVisualStyleBackColor = true;
            this.btn_Scan1.Click += new System.EventHandler(this.btn_Scan1_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.AutoSize = true;
            this.btn_Modify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Modify.Font = new System.Drawing.Font("宋体", 10F);
            this.btn_Modify.Location = new System.Drawing.Point(256, 63);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(52, 30);
            this.btn_Modify.TabIndex = 32;
            this.btn_Modify.Text = "修改";
            this.btn_Modify.UseVisualStyleBackColor = true;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // nud_CurrentCommunicationChannel
            // 
            this.nud_CurrentCommunicationChannel.Location = new System.Drawing.Point(144, 64);
            this.nud_CurrentCommunicationChannel.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nud_CurrentCommunicationChannel.Name = "nud_CurrentCommunicationChannel";
            this.nud_CurrentCommunicationChannel.Size = new System.Drawing.Size(104, 28);
            this.nud_CurrentCommunicationChannel.TabIndex = 31;
            this.tt_tip.SetToolTip(this.nud_CurrentCommunicationChannel, "范围：40~100");
            this.nud_CurrentCommunicationChannel.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(8, 68);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 20);
            this.label4.TabIndex = 29;
            this.label4.Text = "当前通信信道：";
            // 
            // HCISettingsDlg
            // 
            this.ClientSize = new System.Drawing.Size(408, 682);
            this.Controls.Add(this.tlp_Global);
            this.Font = new System.Drawing.Font("宋体", 11F);
            this.Name = "HCISettingsDlg";
            this.Text = "HCI设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortSettingsDlg_FormClosing);
            this.Load += new System.EventHandler(this.SerialPortSettingsDlg_Load);
            this.Controls.SetChildIndex(this.tlp_Global, 0);
            this.tlp_Global.ResumeLayout(false);
            this.gp_Host.ResumeLayout(false);
            this.gp_Host.PerformLayout();
            this.gp_CommunicationMode.ResumeLayout(false);
            this.gp_CommunicationMode.PerformLayout();
            this.gp_SerialConnectionMode.ResumeLayout(false);
            this.gp_SerialConnectionMode.PerformLayout();
            this.gp_Slave0.ResumeLayout(false);
            this.gp_Slave0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SlaveDevices)).EndInit();
            this.gp_Slave1.ResumeLayout(false);
            this.gp_Slave1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CurrentCommunicationChannel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlp_Global;
        private System.Windows.Forms.GroupBox gp_Host;
        private System.Windows.Forms.GroupBox gp_SerialConnectionMode;
        private System.Windows.Forms.RadioButton radbtn_WirelessSerialPort;
        private System.Windows.Forms.RadioButton radbtn_WiredSerialPort;
        private System.Windows.Forms.Label lbl_PortStatus;
        private CustomControls.ToggleButton tglbtn_Switch;
        private System.Windows.Forms.ComboBox cbb_BaudRates;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbb_Ports;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.GroupBox gp_CommunicationMode;
        private System.Windows.Forms.RadioButton radbtn_BluetoothToSerial;
        private System.Windows.Forms.RadioButton radbtn_ESBToSerial;
        private System.Windows.Forms.GroupBox gp_Slave0;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.Button btn_Scan0;
        private System.Windows.Forms.Label lbl_ScanStatus0;
        private System.Windows.Forms.DataGridView dgv_SlaveDevices;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_TxtSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvc_TxtFrom;
        private System.Windows.Forms.DataGridViewButtonColumn dgvc_BtnDelete;
        private System.Windows.Forms.ToolTip tt_tip;
        private System.Windows.Forms.GroupBox gp_Slave1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nud_CurrentCommunicationChannel;
        private System.Windows.Forms.RadioButton radbtn_DirectConnection;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.Button btn_Scan1;
        private System.Windows.Forms.Label lbl_ScanStatus1;
    }
}
