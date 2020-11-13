using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.App.Extensions
{
    public static class BoolExtensions
    {
        public static bool Yes(this bool value) => value;

        public static bool No(this bool value) => !value;

    }
}
