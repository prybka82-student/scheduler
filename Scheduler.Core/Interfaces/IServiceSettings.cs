using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Interfaces
{
    public interface IServiceSettings
    {
        string InputFilePath { get; }
        string OutputFilePath { get; }
        string TimeInterval { get; }
        string DataTemplate { get; }
        ILogger Logger { get; }
        IScheduler Scheduler { get; }
        IJob Job { get; }
        ICsvHelper CsvHelper { get; }
        IMailer Mailer { get; }
    }
}
