using System;
using System.Collections;

namespace GNDView.Library.Plugin
{
    /// <summary>
    /// Extension类的集合
    /// </summary>
    public class ExtensionCollection : ICollection
    {
        #region        变量 start
        /// <summary>
        /// list对象,内部存储extension对象
        /// </summary>
        private ArrayList list = new ArrayList();
        #endregion 变量 end

        #region        构造与析构 start
        /// <summary>
        /// 初始化 ExtensionCollection 类的新实例
        /// </summary>
        public ExtensionCollection()
        {

        }

        /// <summary>
        /// 初始化 ExtensionCollection 类的新实例
        /// </summary>
        /// <param name="extensions">Extension类型数组,追加对象</param>
        public ExtensionCollection(Extension[] extensions)
            : this()
        {
            this.AddRange(extensions);
        }
        #endregion 构造与析构 end

        #region        属性 start
        /// <summary>
        /// 从ExtensionCollection集合中检索Extension对象
        /// </summary>
        /// <param name="index">按索引</param>
        public Extension this[int index]
        {
            get
            {
                return this.list[index] as Extension;
            }
            set
            {
                this.list[index] = value;
            }
        }

        /// <summary>
        /// 从ExtensionCollection集合中检索Extension对象
        /// </summary>
        /// <param name="name">按名称</param>
        public Extension this[string name]
        {
            get
            {
                foreach (Extension extension in this.list)
                {
                    if (extension.Name == name)
                    {
                        return extension;
                    }
                }
                return this.list[-1] as Extension;
            }
        }
        
        /// <summary>
        /// 添加一个Extension对象到集合
        /// </summary>
        public int Add(Extension extension)
        {
            return this.list.Add(extension);
        }

        /// <summary>
        /// 添加一组Extension对象到集合
        /// </summary>
        public void AddRange(ICollection extensions)
        {
            this.list.AddRange(extensions);
        }
        
        /// <summary>
        /// 从集合中移除一个Extension对象
        /// </summary>
        /// <param name="extension"></param>
        public void Remove(Extension extension)
        {
            this.list.Remove(extension);
        }
        
        /// <summary>
        /// 从集合中移除指定索引处的Extension对象
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        /// <summary>
        /// Extension对象总数
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