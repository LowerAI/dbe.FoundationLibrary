namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    partial class InputBox
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
            this.txt_Input = new System.Windows.Forms.TextBox();
            this.lbl_Info = new System.Windows.Forms.Label();
            this.ep_Input = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ep_Input)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_Input
            // 
            this.txt_Input.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_Input.Location = new System.Drawing.Point(4, 28);
            this.txt_Input.Name = "txt_Input";
            this.txt_Input.Size = new System.Drawing.Size(315, 27);
            this.txt_Input.TabIndex = 0;
            this.txt_Input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Input_KeyDown);
            this.txt_Input.Validating += new System.ComponentModel.CancelEventHandler(this.txt_Input_Validating);
            // 
            // lbl_Info
            // 
            this.lbl_Info.BackColor = System.Drawing.SystemColors.Info;
            this.lbl_Info.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Info.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbl_Info.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Info.ForeColor = System.Drawing.Color.Gray;
            this.lbl_Info.Location = new System.Drawing.Point(4, 56);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(315, 21);
            this.lbl_Info.TabIndex = 1;
            this.lbl_Info.Text = "[Enter]确认 | [Esc]取消 |WLS(序列号)";
            this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ep_Input
            // 
            this.ep_Input.ContainerControl = this;
            // 
            // InputBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(323, 83);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.txt_Input);
            this.Name = "InputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "InputBox";
            this.Controls.SetChildIndex(this.txt_Input, 0);
            this.Controls.SetChildIndex(this.lbl_Info, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ep_Input)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Input;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.ErrorProvider ep_Input;
    }
}