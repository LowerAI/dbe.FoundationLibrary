namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class AboutDlg
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
            this.lbl_Version = new System.Windows.Forms.Label();
            this.lbl_Copyright = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_ProjectName = new System.Windows.Forms.Label();
            this.lbl_Company = new System.Windows.Forms.Label();
            this.picbox_Trademark = new System.Windows.Forms.PictureBox();
            this.lnklbl_Website = new System.Windows.Forms.LinkLabel();
            this.lbl_Phone = new System.Windows.Forms.Label();
            this.lbl_EmailKey = new System.Windows.Forms.Label();
            this.lnklbl_Email = new System.Windows.Forms.LinkLabel();
            this.lbl_CompiledOn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Trademark)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Version
            // 
            this.lbl_Version.AutoSize = true;
            this.lbl_Version.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Version.Location = new System.Drawing.Point(170, 56);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(72, 28);
            this.lbl_Version.TabIndex = 7;
            this.lbl_Version.Text = "版本：";
            this.lbl_Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Copyright
            // 
            this.lbl_Copyright.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_Copyright.AutoSize = true;
            this.lbl_Copyright.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Copyright.Location = new System.Drawing.Point(150, 212);
            this.lbl_Copyright.Name = "lbl_Copyright";
            this.lbl_Copyright.Size = new System.Drawing.Size(120, 26);
            this.lbl_Copyright.TabIndex = 8;
            this.lbl_Copyright.Text = "Copyright ©";
            this.lbl_Copyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(138, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 32);
            this.label6.TabIndex = 9;
            this.label6.Text = "所有版权保留";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ProjectName
            // 
            this.lbl_ProjectName.AutoSize = true;
            this.lbl_ProjectName.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold);
            this.lbl_ProjectName.Location = new System.Drawing.Point(176, 28);
            this.lbl_ProjectName.Name = "lbl_ProjectName";
            this.lbl_ProjectName.Size = new System.Drawing.Size(79, 30);
            this.lbl_ProjectName.TabIndex = 11;
            this.lbl_ProjectName.Text = "项目名";
            this.lbl_ProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Company
            // 
            this.lbl_Company.AutoSize = true;
            this.lbl_Company.Font = new System.Drawing.Font("宋体", 12F);
            this.lbl_Company.ForeColor = System.Drawing.Color.Indigo;
            this.lbl_Company.Location = new System.Drawing.Point(148, 105);
            this.lbl_Company.Name = "lbl_Company";
            this.lbl_Company.Size = new System.Drawing.Size(249, 20);
            this.lbl_Company.TabIndex = 12;
            this.lbl_Company.TabStop = true;
            this.lbl_Company.Text = "xx有限公司";
            this.lbl_Company.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picbox_Trademark
            // 
            this.picbox_Trademark.BackColor = System.Drawing.Color.Transparent;
            this.picbox_Trademark.Image = global::dbe.FoundationLibrary.Windows.Properties.Resources.ind01;
            this.picbox_Trademark.Location = new System.Drawing.Point(16, 130);
            this.picbox_Trademark.Name = "picbox_Trademark";
            this.picbox_Trademark.Size = new System.Drawing.Size(100, 56);
            this.picbox_Trademark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbox_Trademark.TabIndex = 3;
            this.picbox_Trademark.TabStop = false;
            // 
            // lnklbl_Website
            // 
            this.lnklbl_Website.AutoSize = true;
            this.lnklbl_Website.Font = new System.Drawing.Font("宋体", 12F);
            this.lnklbl_Website.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnklbl_Website.Location = new System.Drawing.Point(167, 133);
            this.lnklbl_Website.Name = "lnklbl_Website";
            this.lnklbl_Website.Size = new System.Drawing.Size(209, 20);
            this.lnklbl_Website.TabIndex = 13;
            this.lnklbl_Website.TabStop = true;
            this.lnklbl_Website.Text = "http://xxx.com";
            this.lnklbl_Website.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnklbl_Website.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklbl_Website_LinkClicked);
            // 
            // lbl_Phone
            // 
            this.lbl_Phone.AutoSize = true;
            this.lbl_Phone.Font = new System.Drawing.Font("宋体", 12F);
            this.lbl_Phone.Location = new System.Drawing.Point(162, 158);
            this.lbl_Phone.Name = "lbl_Phone";
            this.lbl_Phone.Size = new System.Drawing.Size(219, 20);
            this.lbl_Phone.TabIndex = 14;
            this.lbl_Phone.TabStop = true;
            this.lbl_Phone.Text = "电话:+86 021-61630266";
            this.lbl_Phone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EmailKey
            // 
            this.lbl_EmailKey.AutoSize = true;
            this.lbl_EmailKey.Font = new System.Drawing.Font("宋体", 12F);
            this.lbl_EmailKey.Location = new System.Drawing.Point(151, 184);
            this.lbl_EmailKey.Name = "lbl_EmailKey";
            this.lbl_EmailKey.Size = new System.Drawing.Size(59, 20);
            this.lbl_EmailKey.TabIndex = 16;
            this.lbl_EmailKey.TabStop = true;
            this.lbl_EmailKey.Text = "邮箱:";
            this.lbl_EmailKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnklbl_Email
            // 
            this.lnklbl_Email.AutoSize = true;
            this.lnklbl_Email.Font = new System.Drawing.Font("宋体", 12F);
            this.lnklbl_Email.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnklbl_Email.Location = new System.Drawing.Point(205, 184);
            this.lnklbl_Email.Name = "lnklbl_Email";
            this.lnklbl_Email.Size = new System.Drawing.Size(189, 20);
            this.lnklbl_Email.TabIndex = 17;
            this.lnklbl_Email.TabStop = true;
            this.lnklbl_Email.Text = "info@xxx.com";
            this.lnklbl_Email.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnklbl_Email.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklbl_Email_LinkClicked);
            // 
            // lbl_CompiledOn
            // 
            this.lbl_CompiledOn.AutoSize = true;
            this.lbl_CompiledOn.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Italic);
            this.lbl_CompiledOn.Location = new System.Drawing.Point(173, 80);
            this.lbl_CompiledOn.Name = "lbl_CompiledOn";
            this.lbl_CompiledOn.Size = new System.Drawing.Size(85, 19);
            this.lbl_CompiledOn.TabIndex = 18;
            this.lbl_CompiledOn.TabStop = true;
            this.lbl_CompiledOn.Text = "编译于：";
            this.lbl_CompiledOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutDlg
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(430, 264);
            this.Controls.Add(this.lbl_CompiledOn);
            this.Controls.Add(this.lnklbl_Email);
            this.Controls.Add(this.lbl_EmailKey);
            this.Controls.Add(this.lbl_Phone);
            this.Controls.Add(this.lnklbl_Website);
            this.Controls.Add(this.lbl_Company);
            this.Controls.Add(this.lbl_ProjectName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl_Copyright);
            this.Controls.Add(this.lbl_Version);
            this.Controls.Add(this.picbox_Trademark);
            this.Name = "AboutDlg";
            this.Text = "关于{0}";
            this.Load += new System.EventHandler(this.AboutVibeViewDlg_Load);
            this.Controls.SetChildIndex(this.picbox_Trademark, 0);
            this.Controls.SetChildIndex(this.lbl_Version, 0);
            this.Controls.SetChildIndex(this.lbl_Copyright, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.lbl_ProjectName, 0);
            this.Controls.SetChildIndex(this.lbl_Company, 0);
            this.Controls.SetChildIndex(this.lnklbl_Website, 0);
            this.Controls.SetChildIndex(this.lbl_Phone, 0);
            this.Controls.SetChildIndex(this.lbl_EmailKey, 0);
            this.Controls.SetChildIndex(this.lnklbl_Email, 0);
            this.Controls.SetChildIndex(this.lbl_CompiledOn, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Trademark)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_Trademark;
        private System.Windows.Forms.Label lbl_Version;
        private System.Windows.Forms.Label lbl_Copyright;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_ProjectName;
        private System.Windows.Forms.Label lbl_Company;
        private System.Windows.Forms.LinkLabel lnklbl_Website;
        private System.Windows.Forms.Label lbl_Phone;
        private System.Windows.Forms.Label lbl_EmailKey;
        private System.Windows.Forms.LinkLabel lnklbl_Email;
        private System.Windows.Forms.Label lbl_CompiledOn;
    }
}