using Scheduler.App.Entities;
using Scheduler.App.Extensions;
using Scheduler.App.Interfaces;
using Scheduler.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Impl.Scheduler
{
    public class Scheduler : IScheduler
    {
        ILogger _logger;
        IJob _job;
        CancellationTokenSource _cts;

        public Scheduler(ILogger logger) => _logger = logger;

        public void AddJob(IJob job)
        {
            _job = job;
            _cts = new CancellationTokenSource();
            _job.CancellationToken = _cts.Token;
        }

        public async Task StartAsync()
        {
            //https://medium.com/@NitinManju/a-simple-scheduled-task-using-c-and-net-c9d3230769ea

            if (_job == null) return;
            
            Console.WriteLine("Press Ctrl + C to stop");
            Console.CancelKeyPress += CancelKeyPress;

            await Task.Run(async () =>
            {
                while (_job.CancellationToken.IsCancellationRequested.No())
                {
                    await _job.DoWorkAsync();

                    _logger.Debug($"\n{new string('=', 20)}\nJob done\n{new string('=', 20)}\n\n\n");
                    
                    await Task.Delay(_job.CronInterval.ToTimeSpan());
                }
            }, _job.CancellationToken);

        }

        private void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                _cts.Cancel();
        }
    }
}
