using System;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 绑定扩展
    /// </summary>
    public static class BindingExtension
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="control"></param>
        /// <param name="binding"></param>
        /// <exception cref="NullReferenceException"></exception>
        public static void Binding(this Control control, Binding binding)
        {
            if (control == null || binding == null)
                throw new NullReferenceException();
            else
                control.DataBindings.Add(binding);
        }

        // 控件失去焦点时写入值到数据源
        private static void Control_LostFocus(object sender, EventArgs e)
        {
            foreach (Binding v in (sender as Control).DataBindings)
            {
                v.WriteValue();
            }
        }

        /// <summary>
        /// 双路绑定
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="propertyName">控件的属性名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据源中的数据成员</param>
        /// <param name="formattingEnabled">是否启用格式化</param>
        /// <param name="formatString">格式化字符串</param>
        /// <param name="nullValue">当数据源包含object值时，值为null的替换值</param>
        /// <param name="trigger">数据源更新的触发方式</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void TwoWayBinding(
           this Control control,
           string propertyName,
           object dataSource,
           string dataMember,
           bool formattingEnabled = false,
           string formatString = null,
           object nullValue = null,
           DataSourceUpdateTrigger trigger = DataSourceUpdateTrigger.OnPropertyChanged)
        {
            if (control == null
                || propertyName == null
                || dataSource == null
                || dataMember == null)
                throw new NullReferenceException();
            else
            {
                Binding binding = new Binding(propertyName, dataSource, dataMember);
                binding.FormattingEnabled = formattingEnabled;
                binding.FormatString = formatString;
                binding.NullValue = nullValue;
                if (trigger.Equals(DataSourceUpdateTrigger.OnLostFocus))
                {
                    binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
                    control.LostFocus += Control_LostFocus;
                }
                else
                    binding.DataSourceUpdateMode = (DataSourceUpdateMode)trigger;
                control.DataBindings.Add(binding);
            }
        }

        /// <summary>
        /// 单路绑定到数据源
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="propertyName">控件的属性名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据源中的数据成员</param>
        /// <param name="formattingEnabled">是否启用格式化</param>
        /// <param name="formatString">格式化字符串</param>
        /// <param name="nullValue">当数据源包含object值时，值为null的替换值</param>
        /// <param name="trigger">数据源更新的触发方式</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void OneWayToSourceBinding(
            this Control control,
            string propertyName,
            object dataSource,
            string dataMember,
            bool formattingEnabled = false,
            string formatString = null,
            object nullValue = null,
            DataSourceUpdateTrigger trigger = DataSourceUpdateTrigger.OnPropertyChanged)
        {
            if (control == null
                || propertyName == null
                || dataSource == null
                || dataMember == null)
                throw new NullReferenceException();
            else
            {
                Binding binding = new Binding(propertyName, dataSource, dataMember,
                    formattingEnabled: formattingEnabled,
                    formatString: formatString,
                    nullValue: nullValue,
                    dataSourceUpdateMode: DataSourceUpdateMode.OnValidation);
                binding.FormattingEnabled = formattingEnabled;
                binding.FormatString = formatString;
                binding.NullValue = nullValue;
                if (trigger.Equals(DataSourceUpdateTrigger.OnLostFocus))
                {
                    binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
                    control.LostFocus += Control_LostFocus;
                }
                else
                    binding.DataSourceUpdateMode = (DataSourceUpdateMode)trigger;
                binding.ControlUpdateMode = ControlUpdateMode.Never;
                control.DataBindings.Add(binding);
            }
        }

        /// <summary>
        /// 单路绑定到控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="propertyName">控件的属性名</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据源中的数据成员</param>
        /// <param name="formattingEnabled">是否启用格式化</param>
        /// <param name="formatString">格式化字符串</param>
        /// <param name="nullValue">当数据源包含object值时，值为null的替换值</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void OneWayBinding(
            this Control control,
            string propertyName,
            object dataSource,
            string dataMember,
            bool formattingEnabled = false,
            string formatString = null,
            object nullValue = null)
        {
            if (control == null
                || propertyName == null
                || dataSource == null
                || dataMember == null)
                throw new NullReferenceException();
            else
            {
                Binding binding = new Binding(propertyName, dataSource, dataMember,
                    formattingEnabled: formattingEnabled,
                    formatString: formatString,
                    nullValue: nullValue,
                    dataSourceUpdateMode: DataSourceUpdateMode.Never);
                binding.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
                control.DataBindings.Add(binding);
            }
        }
    }
}