using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 表示命令注册管理器。
    /// </summary>
    public static class CommandBindingManager
    {
        private static readonly List<CommandTarget> targets = new List<CommandTarget>();

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="control">绑定目标。</param>
        /// <param name="commandBinding">命令绑定源。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(Control control, CommandBinding commandBinding)
        {
            var target = new CommandTarget(control, commandBinding);
            targets.Add(target);
            return target;
        }

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="stripItem">绑定目标。</param>
        /// <param name="commandBinding">命令绑定源。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(ToolStripItem stripItem, CommandBinding commandBinding)
        {
            var target = new CommandTarget(stripItem, commandBinding);
            targets.Add(target);
            return target;
        }

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="control">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(Control control, Command command)
        {
            return Register(control, new CommandBinding(command));
        }
        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="stripItem">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(ToolStripItem stripItem, Command command)
        {
            return Register(stripItem, new CommandBinding(command));
        }

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="control">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <param name="source">构造命令参数的源。</param>
        /// <param name="propertyName">构造命令参数的名称。</param>
        /// <param name="booleanOppose">若值为System.Boolean值时，是否取反向值。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(Control control, Command command, object source, string propertyName, bool booleanOppose = false)
        {
            var commandBinding = new CommandBinding(command, source, propertyName, booleanOppose);
            return Register(control, commandBinding);
        }

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="stripItem">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <param name="source">构造命令参数的源。</param>
        /// <param name="propertyName">构造命令参数的名称。</param>
        /// <param name="booleanOppose">若值为System.Boolean值时，是否取反向值。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(ToolStripItem stripItem, Command command, object source, string propertyName, bool booleanOppose = false)
        {
            var commandBinding = new CommandBinding(command, source, propertyName, booleanOppose);
            return Register(stripItem, commandBinding);
        }

        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="control">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <param name="staticSourceType">静态类类型。</param>
        /// <param name="propertyName">构造命令参数的名称。</param>
        /// <param name="booleanOppose">若值为System.Boolean值时，是否取反向值。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(Control control, Command command, Type staticSourceType, string propertyName, bool booleanOppose = false)
        {
            var commandBinding = new CommandBinding(command, staticSourceType, propertyName, booleanOppose);
            return Register(control, commandBinding);
        }
        /// <summary>
        /// 注册命令道指定的命令控件。
        /// </summary>
        /// <param name="stripItem">绑定目标。</param>
        /// <param name="command">绑定命令。</param>
        /// <param name="staticSourceType">静态类类型。</param>
        /// <param name="propertyName">构造命令参数的名称。</param>
        /// <param name="booleanOppose">若值为System.Boolean值时，是否取反向值。</param>
        /// <returns>返回一个命令目标实例。</returns>
        public static CommandTarget Register(ToolStripItem stripItem, Command command, Type staticSourceType, string propertyName, bool booleanOppose = false)
        {
            var commandBinding = new CommandBinding(command, staticSourceType, propertyName, booleanOppose);
            return Register(stripItem, commandBinding);
        }

        /// <summary>
        /// 反注册命令。
        /// </summary>
        /// <param name="target">注销的命令目标。</param>
        public static void Unregister(CommandTarget target)
        {
            if (target == null)
            {
                return;
            }
            if (targets.Contains(target))
            {
                targets.Remove(target);
                target.Dispose();
            }
        }
    }
}