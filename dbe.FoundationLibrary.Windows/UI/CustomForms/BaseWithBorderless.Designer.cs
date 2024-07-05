namespace dbe.FoundationLibrary.Windows.UI.CustomForms
{
    partial class BaseWithBorderless
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
            this.lbl_Close = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Close
            // 
            this.lbl_Close.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl_Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbl_Close.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Bold);
            this.lbl_Close.Location = new System.Drawing.Point(538, 0);
            this.lbl_Close.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Close.Name = "lbl_Close";
            this.lbl_Close.Size = new System.Drawing.Size(24, 24);
            this.lbl_Close.TabIndex = 1;
            this.lbl_Close.Text = "×";
            this.lbl_Close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Close.Click += new System.EventHandler(this.lbl_Close_Click);
            this.lbl_Close.MouseLeave += new System.EventHandler(this.lbl_Close_MouseLeave);
            this.lbl_Close.MouseHover += new System.EventHandler(this.lbl_Close_MouseHover);
            // 
            // BaseWithBorderless
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(560, 450);
            this.Controls.Add(this.lbl_Close);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BaseWithBorderless";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BaseWithBorderless";
            this.Load += new System.EventHandler(this.BaseWithBorderless_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BaseWithBorderless_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BaseWithBorderless_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BaseWithBorderless_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Close;
    }
}