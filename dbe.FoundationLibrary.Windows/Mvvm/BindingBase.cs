using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    public abstract class BindingBase : INotifyPropertyChanged
    {
        protected void SetValue<T>(ref T target, T value, [CallerMemberName] string propertyName = null)
        {
            if ((target == null && value != null) || (target != null && !target.Equals(value)))
            {
                target = value;
                //if (Application.OpenForms.Count == 0) return;
                //var form = Application.OpenForms[0];
                //if (form != null && form.IsHandleCreated)
                //{
                //    form.Invoke(new Action(() =>
                //    {
                //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                //    }));
                //}
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}