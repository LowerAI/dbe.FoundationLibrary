using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace dbe.FoundationLibrary.Windows.Mvvm
{/// <summary>
 /// 表示命令参数。
 /// </summary>
    public sealed class CommandParameter
    {
        private readonly Type sourceType;
        private readonly object source;
        private readonly object parameterSource;
        private readonly string propertyMember;

        private readonly bool booleanOppose = false;
        private Command command;

        /// <summary>
        /// 获取一个值，该值表示命令参数源。
        /// </summary>
        /// <value>The source.</value>
        public object Source
        {
            get { return source; }
        }

        /// <summary>
        /// 获取一个值，该值表示命令参数的类型若当前非静态类型绑定则为 Source 类型。
        /// </summary>
        /// <value>The type of the source.</value>
        public Type SourceType
        {
            get
            {
                return sourceType;
            }
        }

        /// <summary>
        /// 获取一个值，该值表示命令执行参数。
        /// </summary>
        /// <value>The parameter.</value>
        public object Parameter { get { return ResolvePropertyValue(); } }

        /// <summary>
        /// 获取一个值，该值表示参数所属的命令。
        /// </summary>
        /// <value>The command.</value>
        public Command Command
        {
            get
            {
                return command;
            }
            internal set
            {
                if (value != command)
                {
                    command = value;
                    command.RaiseCommandState(Parameter);
                }
            }
        }

        /// <summary>
        /// 初始化 RelateCommandParameter 新实例。
        /// </summary>
        /// <param name="source">绑定源。</param>
        /// <param name="propertyMember">绑定成员（属性名称）。</param>
        /// <param name="booleanOppose">若值为System.Boolean时，是否取反。</param>
        public CommandParameter(object source, string propertyMember, bool booleanOppose = false)
        {
            this.source = source;
            this.sourceType = source.GetType();
            this.propertyMember = propertyMember;
            this.booleanOppose = booleanOppose;
            BindNotifyObject(source);
        }

        /// <summary>
        /// 初始化 RelateCommandParameter 新实例。
        /// </summary>
        /// <param name="source">绑定源。</param>
        /// <param name="propertyMember">绑定成员（属性名称）。</param>
        /// <param name="booleanOppose">若值为System.Boolean时，是否取反。</param>
        public CommandParameter(object source, Expression<Func<string>> propertyMember, bool booleanOppose = false)
            : this(source, ResolvePropertyName(propertyMember), booleanOppose)
        {

        }

        /// <summary>
        /// 初始化一个可指定静态成员的 RelateCommandParameter 新实例。
        /// </summary>
        /// <param name="staticSourceType">静态类类型。</param>
        /// <param name="propertyMember">绑定成员（属性名称）。</param>
        /// <param name="booleanOppose">若值为System.Boolean时，是否取反。</param>
        public CommandParameter(Type staticSourceType, string propertyMember, bool booleanOppose = false)
        {
            this.sourceType = staticSourceType;
            this.propertyMember = propertyMember;
            this.booleanOppose = booleanOppose;
        }

        private void BindNotifyObject(object source)
        {
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(source.GetType()))
            {
                ((INotifyPropertyChanged)source).PropertyChanged += SourcePropertyChanged;
            }
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyMember)
            {
                if (Command != null)
                {
                    Command.RaiseCommandState(Parameter);
                }
            }
        }

        private object ResolvePropertyValue()
        {
            var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
            if (source == null)
            {
                flags |= System.Reflection.BindingFlags.Static;
            }
            else
            {
                flags |= System.Reflection.BindingFlags.Instance;
            }

            var pro = sourceType.GetProperty(propertyMember, flags);
            if (pro == null)
            {
                throw new MemberAccessException(string.Format("Not found {2} member \"{0}\" in \"{1}\"", propertyMember, sourceType, source == null ? "static" : "instance"));
            }
            if (Type.GetTypeCode(pro.PropertyType) == TypeCode.Boolean)
            {
                if (booleanOppose)
                {
                    return !((Boolean)pro.GetValue(source));
                }
            }
            return pro.GetValue(source);
        }

        private static string ResolvePropertyName(Expression<Func<string>> propertyMember)
        {
            if (propertyMember != null)
            {
                return ((MemberExpression)propertyMember.Body).Member.Name;
            }
            else
            {
                throw new ArgumentNullException("propertyMember");
            }
        }
    }
}