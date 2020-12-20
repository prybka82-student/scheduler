using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Scheduler.Core.Extensions;

namespace Scheduler.Impl.Logger
{
    public class Logger : Scheduler.Core.Interfaces.ILogger
    {
        public Logger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("App Name", "Scheduller app")
                .CreateLogger();
        }

        #region Debug

        public void Debug(string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName);

            Log.Debug(message);
        }

        #endregion

        #region Exception

        public void Exception(Exception exception, string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName, exception);

            Log.Error(exception, message);
        }

        #endregion

        #region Fatal

        public void Fatal(string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName);

            Log.Fatal(message);
        }

        #endregion

        #region Information

        public void Information(string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName);

            Log.Information(message);
        }

        #endregion

        #region Time

        public void Time(TimeSpan time, string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName, time);

            Log.Information(message);
        }

        #endregion

        #region Warning

        public void Warning(string message = "", [CallerMemberName] string callerName = "")
        {
            message = MessageFactory(message, callerName);

            Log.Warning(message);
        }

        #endregion

        #region private auxiliary methods

        private string MessageFactory(string message, string callerName)
        {
            return GenerateMessage(message, "Operation started", callerName);
        }

        private string MessageFactory(string message, string callerName, Exception exception)
        {
            return GenerateMessage(message, $"The following exception occured: {exception.Message}", callerName);
        }

        private string MessageFactory(string message, string callerName, TimeSpan timeSpan)
        {
            return GenerateMessage(message, $"The operation lasted {timeSpan.ToString()}", callerName);
        }

        private string GenerateMessage(string message, string defaultMessage, string callerName)
        {
            var messageBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(callerName).No())
                messageBuilder.Append($"{callerName}: ");

            if (string.IsNullOrEmpty(message))
                messageBuilder.Append(defaultMessage);
            else
                messageBuilder.Append(message);

            return CorrectLastCharacter(messageBuilder).ToString();
        }

        private StringBuilder CorrectLastCharacter(StringBuilder stringBuilder)
        {   
            if (new[] { '.', '!', '?' }.Contains(stringBuilder.ToString().Last()).No())
                stringBuilder.Append('.');

            return stringBuilder;
        }

        #endregion

    }
}
