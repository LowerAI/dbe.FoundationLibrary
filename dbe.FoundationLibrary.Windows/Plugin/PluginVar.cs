using System.Collections;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// (全局变量)插件相关信息
    /// </summary>
    public class PluginVar
    {
        /// <summary>
        /// 全局变量------当前容器加载的插件程序集
        /// </summary>
        public static AssemblyCollection PluginAssemblys;

        /// <summary>
        /// 全局变量
        /// </summary>
        public static Hashtable PluginEnCnNameHashTable;

        /// <summary>
        /// 插件通配符格式,数组
        /// </summary>
        public static string[] PluginNames = new string[] { "*Plugin*.dll" };

        /// <summary>
        /// 插件容器
        /// </summary>
        public static PluginFillContainer GlobalContainer;

        /// <summary>
        /// 最大插件索引号
        /// </summary>
        public static int GlobalMaxPluginIndex = 0;
    }
}