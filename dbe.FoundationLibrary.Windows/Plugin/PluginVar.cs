using System.Collections;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// (ȫ�ֱ���)��������Ϣ
    /// </summary>
    public class PluginVar
    {
        /// <summary>
        /// ȫ�ֱ���------��ǰ�������صĲ������
        /// </summary>
        public static AssemblyCollection PluginAssemblys;

        /// <summary>
        /// ȫ�ֱ���
        /// </summary>
        public static Hashtable PluginEnCnNameHashTable;

        /// <summary>
        /// ���ͨ�����ʽ,����
        /// </summary>
        public static string[] PluginNames = new string[] { "*Plugin*.dll" };

        /// <summary>
        /// �������
        /// </summary>
        public static PluginFillContainer GlobalContainer;

        /// <summary>
        /// �����������
        /// </summary>
        public static int GlobalMaxPluginIndex = 0;
    }
}