using System;
using System.Collections;
using System.Reflection;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// 程序集集合
    /// </summary>
    public class AssemblyCollection : ICollection
    {
        #region        变量 start
        /// <summary>
        /// list对象
        /// </summary>
        private ArrayList list = new ArrayList();
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 初始化 AssemblyCollection 类的新实例
        /// </summary>
        public AssemblyCollection()
        {

        }

        /// <summary>
        /// 初始化 AssemblyCollection 类的新实例
        /// </summary>
        /// <param name="extensions">Assembly 数组</param>
        public AssemblyCollection(Assembly[] extensions)
            : this()
        {
            this.AddRange(extensions);
        }
        #endregion 构造与析构 end

        #region        索引 start
        /// <summary>
        /// 从AssemblyCollection集合中检索Assembly对象
        /// </summary>
        /// <param name="index">按索引</param>
        public Assembly this[int index]
        {
            get
            {
                return this.list[index] as Assembly;
            }
            set
            {
                this.list[index] = value;
            }
        }

        /// <summary>
        /// 从AssemblyCollection集合中检索Assembly对象
        /// </summary>
        /// <param name="name">按名称</param>
        public Assembly this[string name]
        {
            get
            {
                foreach (Assembly assembly in this.list)
                {
                    if (assembly.GetName().Name == name)
                    {
                        return assembly;
                    }
                }
                return this.list[-1] as Assembly;
            }
            set
            {
                Assembly assem;
                foreach (Assembly assembly in this.list)
                {
                    if (assembly.GetName().Name == name)
                    {
                        assem = assembly;
                    }
                }
                assem = value;
            }
        }
        #endregion 索引 end

        #region        属性 start
        /// <summary>
        /// Assembly对象总数
        /// </summary>
        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        /// <summary>
        ///获取一个值，该值指示是否同步对 ArrayList 的访问（线程安全）。
        ///如果对 ArrayList 的访问是同步的（线程安全），则为 true；否则为 false。默认为 false。
        /// </summary>
        public virtual bool IsSynchronized
        {
            get
            {
                return this.list.IsSynchronized;
            }
        }

        /// <summary>
        ///获取可用于同步 ArrayList 访问的对象。
        /// </summary>
        public virtual object SyncRoot
        {
            get
            {
                return this.list.SyncRoot;
            }
        }
        #endregion 属性 end

        #region        公开方法 start
        /// <summary>
        /// 添加一个Assembly对象到集合
        /// </summary>
        public int Add(Assembly assembly)
        {
            return this.list.Add(assembly);
        }

        /// <summary>
        /// 添加一组Assembly对象到集合
        /// </summary>
        public void AddRange(ICollection extensions)
        {
            this.list.AddRange(extensions);
        }

        /// <summary>
        /// 从集合中移除一个Assembly对象
        /// </summary>
        /// <param name="assembly"></param>
        public void Remove(Assembly assembly)
        {
            this.list.Remove(assembly);
        }

        /// <summary>
        /// 从集合中移除指定索引处的Assembly对象
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        /// <summary>
        /// 清除集合中的所有对象
        /// </summary>
        public void Clear()
        {
            this.list.Clear();
        }

        /// <summary>
        /// 创建集合的浅表副本
        /// </summary>
        public void Clone()
        {
            this.list.Clone();
        }

        /// <summary>
        /// 从目标数组的开头开始将整个 ArrayList 复制到兼容的一维 System.Array 中。
        /// </summary>
        /// <param name="array">作为从 ArrayList 复制的元素的目标位置的一维 System.Array。System.Array必须具有从零开始的索引。</param>
        public void CopyTo(Array array)
        {
            this.list.CopyTo(array);
        }

        /// <summary>
        /// 从目标数组的指定索引处开始将整个 ArrayList 复制到兼容的一维 System.Array。
        /// </summary>
        /// <param name="array">作为从 ArrayList 复制的元素的目标位置的一维 System.Array。System.Array必须具有从零开始的索引。</param>
        /// <param name="arrayIndex">array 中从零开始的索引，在此处开始复制。</param>
        public void CopyTo(Array array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 从目标数组的指定索引处开始，将一定范围的元素从 ArrayList 复制到兼容的一维 System.Array中。
        /// </summary>
        /// <param name="index">源 ArrayList 中复制开始位置的从零开始的索引。。</param>
        /// <param name="array">作为从 ArrayList 复制的元素的目标位置的一维 System.Array。System.Array必须具有从零开始的索引。</param>
        /// <param name="arrayIndex">array 中从零开始的索引，在此处开始复制。</param>
        /// <param name="count">要复制的元素数。</param>
        public void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            this.list.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// 返回整个 ArrayList 的一个枚举数。
        /// </summary>
        public virtual IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
        #endregion 公开方法 end
    }
}