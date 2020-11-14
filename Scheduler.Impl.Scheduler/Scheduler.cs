using Hangfire;
using Hangfire.MemoryStorage;
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
        BackgroundJobServerOptions _serverOptions;

        public Scheduler(ILogger logger, string sqlConnectionString)
        {
            _logger = logger;
            _jobs = new List<string>();

            _serverOptions = new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 5
            };

            logger.Debug("Configuring Hangfire...");

            GlobalConfiguration
                .Configuration
                .UseSqlServerStorage(sqlConnectionString);
                        
        }

        public void AddOrUpdateJob(IJob job, string jobName, CancellationToken cancellationToken, string interval)
        {
            _logger.Debug($"Adding job '{jobName}'");

            _jobs.Add(jobName);

            //var id = BackgroundJob.Enqueue(() => job.DoWorkAsync(cancellationToken));
            //var id = BackgroundJob.Schedule(() => job.DoWorkAsync(cancellationToken), TimeSpan.FromSeconds(0));

            //_logger.Debug($"Fire and forget background job id: {id}");

            RecurringJob.AddOrUpdate(
                recurringJobId: jobName,
                methodCall: () => job.DoWorkAsync(cancellationToken),
                cronExpression: interval);
        }

        public void StartJobs(CancellationToken cancellationToken)
        {
            //using (new BackgroundJobServer(_serverOptions));            

            foreach (var jobName in _jobs)
            {
                if (cancellationToken.IsCancellationRequested.No())
                {
                    _logger.Debug($"Triggering job '{jobName}'");
                    RecurringJob.Trigger(jobName);
                }
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
