using System;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace dbe.FoundationLibrary.Core.Util
{
    /// <summary>
    /// 通过反射创建实例
    /// ExpressionTree和Edmit初始化速度基本相同,和直接初始化相比只慢了少许 ,其次是Activator.CreateInstanse方法,
    /// Activator.CreateInstanse<T>方法是最慢的
    /// </summary>
    public class Reflection2Instance
    {
        /// <summary>
        /// 通过ExpressionTree构造委托来初始化
        /// 适用于T和type代表的class完全一致的情况
        /// </summary>
        /// <returns></returns>
        public static Func<T> GetFuncByET<T>()
        {
            Type type = typeof(T);
            var newExp = Expression.New(type);
            Expression<Func<T>> lambdaExp = Expression.Lambda<Func<T>>(newExp, null);
            Func<T> func = lambdaExp.Compile();
            return func;
        }

        /// <summary>
        /// 通过ExpressionTree构造委托来初始化
        /// 适用于T和type代表的class不一致的情况，例如T是接口而type代表的class是继承于T的类
        /// </summary>
        /// <returns></returns>
        public static Func<T> GetFuncByET<T>(Type type)
        {
            var newExp = Expression.New(type);
            Expression<Func<T>> lambdaExp = Expression.Lambda<Func<T>>(newExp, null);
            Func<T> func = lambdaExp.Compile();
            return func;
        }

        /// <summary>
        /// 通过ExpressionTree构造委托来初始化
        /// 适用于只知道type完全不清楚T的情况
        /// </summary>
        /// <returns></returns>
        public static Func<object> GetFuncByET(Type type)
        {
            var newExp = Expression.New(type);
            Expression<Func<object>> lambdaExp = Expression.Lambda<Func<object>>(newExp, null);
            Func<object> func = lambdaExp.Compile();
            return func;
        }

        /// <summary>
        /// 通过Emit构造委托初始化
        /// </summary>
        /// <returns></returns>
        public static Func<T> GetFuncByEmit<T>()
        {
            Type type = typeof(T);
            var ctor = type.GetConstructors()[0];
            DynamicMethod method = new DynamicMethod(String.Empty, typeof(T), null);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        /// <summary>
        /// 通过Emit构造委托初始化
        /// </summary>
        /// <returns></returns>
        public static Func<T> GetFuncByEmit<T>(Type type)
        {
            var ctor = type.GetConstructors()[0];
            DynamicMethod method = new DynamicMethod(String.Empty, typeof(T), null);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        /// <summary>
        /// 通过Emit构造委托初始化
        /// </summary>
        /// <returns></returns>
        public static Func<object> GetFuncByEmit(Type type)
        {
            var ctor = type.GetConstructors()[0];
            DynamicMethod method = new DynamicMethod(String.Empty, type, null);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<object>)) as Func<object>;
        }

        /// <summary>
        /// 调用Activator.CreateInstanse(Type type, param object[] args)初始化
        /// Activator.CreateInstanse(Type type, param object[] args)比Activator.CreateInstanse(Type type)还要快一点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ActivatorCreate<T>() where T : class
        {
            Type type = typeof(T);
            return Activator.CreateInstance(type, null) as T;
        }
    }
}