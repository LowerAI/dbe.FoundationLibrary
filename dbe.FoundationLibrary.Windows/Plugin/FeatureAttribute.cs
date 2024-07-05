using System;
using System.Reflection;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 定义功能部件程序集属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class FeatureAttribute : Attribute
    {
        #region        变量 start
        //private Type entryClass;
        //private object entryObject;
        //private string name;
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 初始化 Agile.Plugin.FeatureAttribute 类的新实例
        /// </summary>
        /// <param name="entryClass">功能部件入口类型名</param>
        public FeatureAttribute(Type entryClass)
            : this(entryClass, "未命名功能部件")
        {
        }

        /// <summary>
        /// 初始化 Agile.Plugin.FeatureAttribute 类的新实例
        /// </summary>
        /// <param name="entryClass">功能部件入口类型名</param>
        /// <param name="name">功能部件名称</param>
        public FeatureAttribute(Type entryClass, string name)
        {
            this.EntryClass = entryClass;
            this.Name = name;
            this.EntryObject = entryClass.InvokeMember("new", BindingFlags.CreateInstance, null, null, null);
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 功能部件入口类型名
        /// </summary>
        public Type EntryClass { get; set; }

        /// <summary>
        /// 插件入口类实例
        /// </summary>
        public object EntryObject { get; set; }

        /// <summary>
        /// 功能部件名称
        /// </summary>
        public string Name { get; set; }
        #endregion 属性 end
    }
}