using System;
using System.Collections.Generic;
using System.Linq;

namespace dbe.FoundationLibrary.Core.Extensions
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 求峰峰值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T GetPeak<T>(this IEnumerable<T> source) where T : IComparable<T>
        {
            if (source == null || !source.Any())
            {
                throw new ArgumentException("Sequence is empty or null.");
            }

            T max = source.Max();
            T min = source.Min();

            return (dynamic)max - (dynamic)min;
        }
    }
}