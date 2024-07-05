using System;
using System.Reflection;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 插件程序集属性类(程序集自定义属性类)
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class PluginAttribute : Attribute
    {
        #region        变量 start
        ///// <summary>
        ///// 插件实体类型 如Asset.Plugins.BasicSetting
        ///// </summary>
        //private Type entryClass;
        ///// <summary>
        ///// 插件实体object对象
        ///// </summary>
        //private Plugin entryObject;
        ///// <summary>
        ///// 插件名
        ///// </summary>
        //private string name;
        ///// <summary>
        ///// 插件加载顺序
        ///// </summary>
        //private int index;
        ///// <summary>
        ///// 插件所在子系统名称
        ///// </summary>
        //private string subsysname;
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 初始化 Agile.Plugin.PluginAttribute 类的新实例
        /// </summary>
        /// <param name="entryClass">插件入口类型名</param>
        /// <param name="name">插件名称</param>
        /// <param name="index">加载插件的顺序号</param>
        public PluginAttribute(Type entryClass, string name, int index)
            : this(entryClass, name, index, "")
        {

        }

        /// <summary>
        /// 初始化 Agile.Plugin.PluginAttribute 类的新实例
        /// </summary>
        /// <param name="entryClass">插件入口类型名</param>
        /// <param name="name">插件名称</param>
        /// <param name="index">加载插件的顺序号</param>
        /// <param name="subsysname">子系统名称</param>
        public PluginAttribute(Type entryClass, string name, int index, string subsysname)
        {
            this.EntryClass = entryClass;
            this.Name = name;
            this.Index = index;
            this.SubSysName = subsysname;
            this.EntryObject = entryClass.InvokeMember("new", BindingFlags.CreateInstance, null, null, null) as Plugin;
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 插件入口类型
        /// </summary>
        public Type EntryClass { get; set; }

        /// <summary>
        /// 插件入口类实例
        /// </summary>
        public Plugin EntryObject { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 加载插件的顺序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 插件所在子系统名称
        /// </summary>
        public string SubSysName { get; set; }
        #endregion 属性 end
    }
}