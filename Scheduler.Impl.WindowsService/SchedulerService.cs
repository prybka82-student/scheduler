using Scheduler.App.Interfaces;
using Scheduler.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Scheduler.Impl.WindowsService
{
    public class SchedulerService : IService
    {
        private readonly string _customerDataFilePath;
        private readonly string _mailDeliveryDirectory;
        private readonly string _messageSendTimeInterval;
        private readonly string _messageTemplate;
        private readonly ILogger _logger;
        private readonly IScheduler _scheduler;
        private readonly IJob _job;
        private readonly ICsvHelper _csvHelper;
        private readonly IMailer _mailer;

        public SchedulerService(IServiceSettings settings)
        {
            _customerDataFilePath = settings.InputFilePath;
            _mailDeliveryDirectory = settings.OutputFilePath;

            _messageSendTimeInterval = settings.TimeInterval;

            _messageTemplate = settings.DataTemplate;

            _logger = settings.Logger;
            _scheduler = settings.Scheduler;
            _job = settings.Job;

            _csvHelper = settings.CsvHelper;

            _mailer = settings.Mailer;
        }

        public async Task StartAsync()
        {
            _scheduler.AddJob(_job);

            await _scheduler.StartAsync();
        }

        public void Stop() => _scheduler.Cancel();
    }
}
