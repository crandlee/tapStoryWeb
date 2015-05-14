using System;
using System.Collections.Generic;
using System.Linq;

namespace tapStoryWebApi.Extensions
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }
        public static IQueryable<T> ForEachQueryable<T>(this IQueryable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
            return source;
        }

    }
}