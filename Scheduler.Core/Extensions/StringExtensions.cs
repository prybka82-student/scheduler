using Scheduler.App.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToPercent(this string percent, int digits = 0)
        {
            if (decimal.TryParse(percent, out decimal val))
            {
                val = val * 100;
                val = Math.Round(val, digits);

                return $"{val}%";
            }

            return percent;
        }

        public static TimeSpan ToTimeSpan(this string cron)
        {
            var parts = cron
                .Replace("*", "0")
                .Split(' ');

            if (parts.Length < 5 || parts.Length > 6) throw new ArgumentOutOfRangeException($"Number of elements in cron {cron} is incorrect");

            if (int.TryParse(parts[0], out int minutes).No()) throw new ArgumentException($"Cannot parse {parts[0]} into int");
            if (int.TryParse(parts[1], out int hours).No()) throw new ArgumentException($"Cannot parse {parts[1]} into int");
            if (int.TryParse(parts[2], out int days).No()) throw new ArgumentException($"Cannot parse {parts[2]} into int");
            if (int.TryParse(parts[3], out int months).No()) throw new ArgumentException($"Cannot parse {parts[3]} into int");
            if (int.TryParse(parts[4], out int years).No()) throw new ArgumentException($"Cannot parse {parts[4]} into int");

            return new TimeSpan(days, hours, minutes, 0);
        }
    }
}
