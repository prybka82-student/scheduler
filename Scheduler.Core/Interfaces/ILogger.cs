using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scheduler.Core.Interfaces
{
    public interface ILogger
    {
        void Debug(string message = "", [CallerMemberName] string callerName = "");
        void Time(TimeSpan time, string message = "", [CallerMemberName] string callerName = "");
        void Exception(Exception exception, string message = "", [CallerMemberName] string callerName = "");
        void Information(string message = "", [CallerMemberName] string callerName = "");
        void Warning(string message = "", [CallerMemberName] string callerName = "");
        void Fatal(string message = "", [CallerMemberName] string callerName = "");
    }
}
