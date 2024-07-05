namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class SoftwareRegistrationDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareRegistrationDlg));
            this.label2 = new System.Windows.Forms.Label();
            this.txt_RegisterCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_OK = new System.Windows.Forms.Button();
            this.txt_MachineCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 27);
            this.label2.TabIndex = 14;
            this.label2.Text = "机器码：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_RegisterCode
            // 
            this.txt_RegisterCode.Location = new System.Drawing.Point(105, 61);
            this.txt_RegisterCode.Name = "txt_RegisterCode";
            this.txt_RegisterCode.Size = new System.Drawing.Size(320, 38);
            this.txt_RegisterCode.TabIndex = 17;
            this.txt_RegisterCode.TextChanged += new System.EventHandler(this.Txt_RegisterCode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 27);
            this.label1.TabIndex = 16;
            this.label1.Text = "注册码：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_OK
            // 
            this.btn_OK.AutoSize = true;
            this.btn_OK.Location = new System.Drawing.Point(433, 35);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(76, 37);
            this.btn_OK.TabIndex = 18;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // txt_MachineCode
            // 
            this.txt_MachineCode.BackColor = System.Drawing.Color.GhostWhite;
            this.txt_MachineCode.Location = new System.Drawing.Point(105, 13);
            this.txt_MachineCode.Name = "txt_MachineCode";
            this.txt_MachineCode.ReadOnly = true;
            this.txt_MachineCode.Size = new System.Drawing.Size(320, 38);
            this.txt_MachineCode.TabIndex = 20;
            // 
            // SoftwareRegistrationDlg
            // 
            this.ClientSize = new System.Drawing.Size(511, 113);
            this.Controls.Add(this.txt_MachineCode);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_RegisterCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("宋体", 16F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoftwareRegistrationDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件激活：";
            this.Load += new System.EventHandler(this.SoftwareRegistrationDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_RegisterCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.TextBox txt_MachineCode;
    }
}
