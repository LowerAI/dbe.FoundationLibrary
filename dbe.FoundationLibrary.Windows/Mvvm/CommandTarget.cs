using System;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 表示命令绑定的执行目标。
    /// </summary>
    public sealed class CommandTarget : IDisposable
    {
        private readonly object target;
        private bool disposed = false;

        /// <summary>
        /// 获取一个值，该值表示目标是否已释放。
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get { return disposed; } }

        /// <summary>
        /// 获取一个值，该值表示目标的命令绑定源。
        /// </summary>
        /// <value>The command binding.</value>
        public CommandBinding CommandBinding { get; private set; }

        /// <summary>
        /// 初始化 CommandTarget 新实例。
        /// </summary> 
        /// <param name="binding">命令绑定源。</param>
        private CommandTarget(CommandBinding binding)
        {
            CommandBinding = binding;
            CommandBinding.Command.CanExecuteChanged += CommandStateChanged;
        }

        /// <summary>
        /// 初始化 CommandTarget 新实例。
        /// </summary>
        /// <param name="control">绑定目标。</param>
        /// <param name="commandBinding">命令绑定源。</param>
        public CommandTarget(Control control, CommandBinding commandBinding) : this(commandBinding)
        {
            target = control;
            control.Click += OnClick;
            var parameter = GetParameterValue();
            control.Enabled = commandBinding.Command.CanExecute(parameter);
        }

        /// <summary>
        /// 初始化 CommandTarget 新实例。
        /// </summary>
        /// <param name="item">绑定目标。</param>
        /// <param name="commandBinding">命令绑定源。</param>
        public CommandTarget(ToolStripItem item, CommandBinding commandBinding) : this(commandBinding)
        {
            target = item;
            item.Click += OnClick;
            var parameter = GetParameterValue();
            item.Enabled = commandBinding.Command.CanExecute(parameter);
        }


        /// <summary>
        /// Handles the <see cref="E:Click" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnClick(object sender, EventArgs e)
        {
            object parameter = null;
            if (CommandBinding.CommandParameter != null)
            {
                parameter = CommandBinding.CommandParameter.Parameter;
            }
            var command = CommandBinding.Command;
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        /// <summary>
        /// Commands the state changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CommandStateChanged(object sender, EventArgs e)
        {
            var property = target.GetType().GetProperty("Enabled");
            if (property != null)
            {
                var parameter = GetParameterValue();
                var enable = CommandBinding.Command.CanExecute(parameter);
                property.SetValue(target, enable);
            }
        }

        private object GetParameterValue()
        {
            if (CommandBinding.CommandParameter == null)
            {
                return null;
            }
            return CommandBinding.CommandParameter.Parameter;
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;
            CommandBindingManager.Unregister(this);
            CommandBinding.Command.CanExecuteChanged -= CommandStateChanged;
            if (target is Control)
            {
                ((Control)target).Click -= OnClick;
            }
            if (target is ToolStripItem)
            {
                ((Control)target).Click -= OnClick;
            }
        }
    }
}