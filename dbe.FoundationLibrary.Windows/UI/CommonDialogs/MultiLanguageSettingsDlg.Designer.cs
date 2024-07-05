namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class MultiLanguageSettingsDlg
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
            this.radbtn_Chinese = new System.Windows.Forms.RadioButton();
            this.radbtn_English = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radbtn_Chinese
            // 
            this.radbtn_Chinese.Checked = true;
            this.radbtn_Chinese.Location = new System.Drawing.Point(25, 48);
            this.radbtn_Chinese.Name = "radbtn_Chinese";
            this.radbtn_Chinese.Size = new System.Drawing.Size(101, 24);
            this.radbtn_Chinese.TabIndex = 2;
            this.radbtn_Chinese.TabStop = true;
            this.radbtn_Chinese.Text = "中文";
            this.radbtn_Chinese.UseVisualStyleBackColor = true;
            // 
            // radbtn_English
            // 
            this.radbtn_English.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radbtn_English.Location = new System.Drawing.Point(151, 48);
            this.radbtn_English.Name = "radbtn_English";
            this.radbtn_English.Size = new System.Drawing.Size(101, 24);
            this.radbtn_English.TabIndex = 3;
            this.radbtn_English.Text = "English";
            this.radbtn_English.UseVisualStyleBackColor = true;
            // 
            // MultiLanguageSettingsDlg
            // 
            this.ClientSize = new System.Drawing.Size(258, 193);
            this.Controls.Add(this.radbtn_English);
            this.Controls.Add(this.radbtn_Chinese);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Name = "MultiLanguageSettingsDlg";
            this.Text = "语言设置";
            this.Controls.SetChildIndex(this.radbtn_Chinese, 0);
            this.Controls.SetChildIndex(this.radbtn_English, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radbtn_Chinese;
        private System.Windows.Forms.RadioButton radbtn_English;
    }
}