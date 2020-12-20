using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNull<T>(this IEnumerable<T> sequence) => sequence == null;

        public static bool IsEmpty<T>(this IEnumerable<T> sequence) => sequence.Any();

        public static IEnumerable<T> Empty<T>(this IEnumerable<T> sequence) => Enumerable.Empty<T>();

        public static IEnumerable<T> ToSingleItemSequence<T>(this T obj) => new[] { obj };

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> sequence) => sequence == null || sequence.Any().No();

    }
}
