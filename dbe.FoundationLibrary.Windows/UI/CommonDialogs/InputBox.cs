using dbe.FoundationLibrary.Windows.UI.CustomForms;

using System.Drawing;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.UI.CommonDialogs
{
    /// <summary>
    /// clsInputBox 的摘要说明。
    /// </summary>
    public partial class InputBox : BaseWithBorderless
    {
        #region    字段 start
        public string input;// 输入的文本
        #endregion 字段 end

        #region    构造与析构 start
        public InputBox(string Title, Point startPosition, string keyInfo = null)
        {
            InitializeComponent();

            this.Text = Title;
            if (startPosition != Point.Empty)
            {
                this.Location = startPosition;
            }
            if (!string.IsNullOrWhiteSpace(keyInfo))
                lbl_Info.Text = keyInfo;
        }
        #endregion 构造与析构 end

        #region    事件处理 start
        //对键盘进行响应
        private void txt_Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!this.ValidateChildren())
                {
                    return;
                }
                this.DialogResult = DialogResult.OK;
                input = txt_Input.Text;
                Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                input = string.Empty;
                Close();
            }
        }

        private void txt_Input_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Input.Text))
            {
                ep_Input.SetError(txt_Input, "输入不能为空!");
                e.Cancel = true;
            }
            else
            {
                ep_Input.Clear();
                e.Cancel = false;
            }
        }
        #endregion 事件处理 end
    }
}