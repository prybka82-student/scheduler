using Scheduler.App.Interfaces;
using Scheduler.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Impl.WindowsService
{
    public class SchedulerServiceSettings : IServiceSettings
    {        
        public string InputFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public string TimeInterval { get; set; }

        public string DataTemplate { get; set; }

        public ILogger Logger { get; set; }

        public IScheduler Scheduler { get; set; }

        public IJob Job { get; set; }

        public ICsvHelper CsvHelper { get; set; }

        public IMailer Mailer { get; set; }

    }
}
