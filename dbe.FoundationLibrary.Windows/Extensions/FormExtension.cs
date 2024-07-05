using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Extensions
{
    public static class FormExtension
    {
        /// <summary>
        /// Sets the form to a dialog appearance
        /// </summary>
        /// <param name="form"></param>
        public static void SetDialogAppearance(this Form form)
        {
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.BackColor = System.Drawing.Color.White;
            //form.Icon = PresentationLayer.Properties.Resources.MVPDemo768Main;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
        }

        /// <summary>
        /// Sets the form to a form appearance
        /// </summary>
        /// <param name="form"></param>
        public static void SetFormAppearance(this Form form)
        {
            SetDialogAppearance(form);
            form.MinimizeBox = true;
        }
    }
}