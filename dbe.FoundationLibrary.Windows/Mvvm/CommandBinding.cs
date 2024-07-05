using System;

namespace dbe.FoundationLibrary.Windows.Mvvm
{
    /// <summary>
    /// 定义命令绑定对象。
    /// </summary>
    public sealed class CommandBinding
    {
        /// <summary>
        /// 获取一个值，该值表示命令绑定的对象。
        /// </summary>
        /// <value>The command.</value>
        public Command Command { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示命令执行参数。
        /// </summary>
        /// <value>The command parameter.</value>
        public CommandParameter CommandParameter { get; private set; }

        /// <summary>
        /// 初始化 <see cref="CommandBinding"/> 新实例。
        /// </summary>
        /// <param name="command">要绑定的命令。</param>
        public CommandBinding(Command command) : this(command, null)
        {

        }

        /// <summary>
        /// 初始化 <see cref="CommandBinding"/> 新实例。
        /// </summary>
        /// <param name="command">要绑定的命令。</param>
        /// <param name="commandParameter">要绑定的命令参数。</param>
        public CommandBinding(Command command, CommandParameter commandParameter)
        {
            this.Command = command;
            this.CommandParameter = commandParameter;
            if (this.CommandParameter != null)
            {
                this.CommandParameter.Command = this.Command;
            }
        }

        /// <summary>
        /// 初始化 <see cref="CommandBinding"/> 新实例。
        /// </summary>
        /// <param name="command">要绑定的命令。</param>
        /// <param name="source">要绑定的命令参数的实例。</param>
        /// <param name="propertyMember">要绑定的命令参数的属性名称。</param>
        /// <param name="booleanOppose">若值为System.Boolean值时，是否取反向值。</param>
        public CommandBinding(Command command, object source, string propertyMember, bool booleanOppose = false)
            : this(command, CreateCommandParameter(source, propertyMember, booleanOppose))
        {

        }

        /// <summary>
        /// 初始化 <see cref="CommandBinding"/> 新实例。
        /// </summary>
        /// <param name="command">要绑定的命令。</param>
        /// <param name="staticSourceType">静态类类型。</param>
        /// <param name="propertyMember">绑定成员（属性名称）。</param>
        /// <param name="booleanOppose">若值为System.Boolean时，是否取反。</param>
        public CommandBinding(Command command, Type staticSourceType, string propertyMember, bool booleanOppose = false)
            : this(command, new CommandParameter(staticSourceType, propertyMember, booleanOppose))
        {
        }

        private static CommandParameter CreateCommandParameter(object source, string propertyMember, bool booleanOppose = false)
        {
            return new CommandParameter(source, propertyMember, booleanOppose);
        }
    }
}