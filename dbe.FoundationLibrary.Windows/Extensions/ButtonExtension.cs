using System;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    public static class ButtonExtension
    {
        /// <summary>
        /// 设置按钮为无边框样式
        /// </summary>
        /// <param name="button"></param>
        public static void SetToBorderless(this Button button)
        {
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = button.BackColor;
            button.FlatAppearance.MouseDownBackColor = button.BackColor;

            button.SetCursorOnHover();
        }

        /// <summary>
        /// 设置作为按钮的下划线的Label的位置
        /// </summary>
        /// <param name="button"></param>
        /// <param name="underlineLabel"></param>
        public static void SetUnderlinePosition(this Button button, Label underlineLabel)
        {
            underlineLabel.Width = button.Bounds.Width - (int)(button.Bounds.Width * .15); ;
            underlineLabel.Left = button.Bounds.Left + (int)(button.Bounds.Width * .08); ;
            underlineLabel.Top = button.Top + button.Height;
        }
    }
}
