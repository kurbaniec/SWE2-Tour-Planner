using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.Utils.Extensions
{
    /// <summary>
    /// Extends the <c>ObservableCollection</c> with some functionality to allow more LINQ operations.
    /// </summary>
    public static class ObservableExtension
    {
        /// <summary>
        /// Allow ForEach-LINQ expression on ObservableCollection.
        /// See: https://stackoverflow.com/a/2519433/12347616
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

        /// <summary>
        /// Allow AddRange-LINQ expression on ObservableCollection.
        /// See: https://stackoverflow.com/a/2184199/12347616
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                coll.Add(item);
            }
        }
    }
}