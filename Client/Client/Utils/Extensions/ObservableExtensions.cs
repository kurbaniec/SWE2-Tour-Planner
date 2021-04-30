using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.Utils.Extensions
{
    public static class ObservableExtension
    {
        // Allow ForEach-LINQ expression on ObservableCollection.
        // See: https://stackoverflow.com/a/2519433/12347616
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            foreach ( var cur in enumerable ) {
                action(cur);
            }
        }
        
        // See: https://stackoverflow.com/a/2184199/12347616
        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                coll.Add(item);
            }
        }
    }
}