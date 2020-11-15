using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Extensions
{
    public static class ICollectionsExtensions
    {
        public static ICollection<T> ToSingleItemSequence<T>(this T obj) => new[] { obj };
    }
}
