using System;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 提供一个主程序扩展点类型
    /// </summary>
    public class Extension
    {
        #region        变量 start
        //private object component;
        private string name;
        private string text;
        //private object tag;
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 初始化 Agile.Plugin.Extension 类的新实例
        /// </summary>
        /// <param name="component">扩展点实体对象实例</param>
        public Extension(object component)
        {
            this.Component = component;
        }

        /// <summary>
        /// 初始化 Agile.Plugin.Extension 类的新实例
        /// </summary>
        /// <param name="component">扩展点实体对象实例</param>
        /// <param name="name">扩展点名称</param>
        public Extension(object component, string name)
            : this(component)
        {
            this.Name = name;
        }

        /// <summary>
        /// 初始化 Agile.Plugin.Extension 类的新实例
        /// </summary>
        /// <param name="component">扩展点实体对象实例</param>
        /// <param name="name">扩展点名称</param>
        /// <param name="text">与扩展点相关联的文本</param>
        public Extension(object component, string name, string text)
            : this(component, name)
        {
            this.text = text;
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 扩展点实体对象实例
        /// </summary>
        public object Component { get; set; }

        /// <summary>
        /// 扩展点名称
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == string.Empty)
                {
                    return this.Component.ToString();
                }
                else
                {
                    return this.name;
                }
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// 与扩展点相关联的文本
        /// </summary>
        public string Text
        {
            get
            {
                if (this.text == string.Empty)
                {
                    return this.Name;
                }
                else
                {
                    return this.text;
                }
            }
            set
            {
                this.text = value;
            }
        }

        /// <summary>
        /// 扩展点控件类型
        /// </summary>
        public Type ControlType
        {
            get
            {
                return this.Component.GetType();
            }
        }

        /// <summary>
        /// 获取或设置一个对象
        /// </summary>
        public object Tag { get; set; }
        #endregion 属性 end
    }
}