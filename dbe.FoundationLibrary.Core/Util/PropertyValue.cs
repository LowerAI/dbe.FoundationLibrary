using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 高效动态获取属性值
    /// 原文链接：https://mp.weixin.qq.com/s/Lo4X0SLP2DgKsK-s7NUxmg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyValue<T>
    {
        private static ConcurrentDictionary<string, MemberGetDelegate> _memberGetDelegate = new ConcurrentDictionary<string, MemberGetDelegate>();
        public T Target { get; private set; }

        delegate object MemberGetDelegate(T obj);

        public PropertyValue(T obj)
        {
            Target = obj;
        }

        //public object Get(string name)
        //{
        //    MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
        //    return memberGet(Target);
        //}

        /// <summary>
        /// 获取指定名称的属性值并转换为TValue类型
        /// </summary>
        /// <typeparam name="TValue">最终输出的类型</typeparam>
        /// <param name="name">属性名称</param>
        /// <returns>TValue类型的属性值</returns>
        public TValue Get<TValue>(string name)
        {
            MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
            dynamic val = memberGet(Target);
            var type = typeof(TValue);
            val = Convert.ChangeType(val, type);
            return val;
        }

        private MemberGetDelegate BuildDelegate(string name)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(name);
            return (MemberGetDelegate)Delegate.CreateDelegate(typeof(MemberGetDelegate), property.GetGetMethod());
        }
    }
}