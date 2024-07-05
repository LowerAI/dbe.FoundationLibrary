using System;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 表示一个可被执行委托的方法的命令。
    /// </summary>
    public sealed class DelegateCommand : Command
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// 初始化 <see cref="DelegateCommand"/> 新实例。
        /// </summary>
        /// <param name="execute">当命令被调用时，指定的方法。</param>
        /// <param name="canExecute">当命令被确定是否能执行时，执行的方法。</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// 定义用于确定此命令是否可以在其当前状态下执行的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。 如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        /// <returns>如果可以执行此命令，则为 true；否则为 false。</returns>
        public override bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute(parameter);
        }

        /// <summary>
        /// 定义在调用此命令时调用的方法。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。 如果此命令不需要传递数据，则该对象可以设置为 null。</param>
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute(parameter);
            }
        }
    }
}