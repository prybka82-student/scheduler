using Hangfire;
using Scheduler.App.Entities;
using Scheduler.App.Extensions;
using Scheduler.App.Interfaces;
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
        List<string> _jobs;

        public Scheduler(ILogger logger)
        {
            _logger = logger;
            _jobs = new List<string>();
        }

        public void AddOrUpdateJob(IJob job, string jobName, CancellationToken cancellationToken, string interval)
        {
            _jobs.Add(jobName);

            RecurringJob.AddOrUpdate(
                recurringJobId: jobName, 
                methodCall: () => job.DoWorkAsync(cancellationToken), 
                cronExpression: interval);
        }

        public void StartJobs(CancellationToken cancellationToken)
        {
            foreach (var jobName in _jobs)
            {
                if (cancellationToken.IsCancellationRequested.No())
                    RecurringJob.Trigger(jobName);
                else
                {
                    var e = new TaskCanceledException();
                    _logger.Exception(e, $"Job called {jobName} was canceled");
                    throw e;
                }
            }
        }

        public void RemoveJob(string jobName)
        {
            RecurringJob.RemoveIfExists(jobName);
        }

        public void StopAndRemoveAllJobs()
        {
            foreach (var jobName in _jobs)
            {
                RecurringJob.RemoveIfExists(jobName);
            }

            _jobs.Clear();
        }

    }
}
