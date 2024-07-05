using System;
using System.Windows.Input;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 定义一个执行的命令。
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// The can executable
        /// </summary>
        private bool canExecutable = true;

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 定义用于确定此命令是否可以在其当前状态下执行的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。 如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        /// <returns>如果可以执行此命令，则为 true；否则为 false。</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// 定义在调用此命令时调用的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。 如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public abstract void Execute(object parameter);


        /// <summary>
        /// 提升命令状态更改。
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        internal protected void RaiseCommandState(object parameter)
        {
            var able = CanExecute(parameter);
            if (able != canExecutable)
            {
                canExecutable = able;
                OnCanExecuteChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// 触发当命令状态更改事件。
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, e);
            }
        }
    }
}