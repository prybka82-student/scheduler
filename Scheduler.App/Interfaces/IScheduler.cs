using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.App.Interfaces
{
    public interface IScheduler
    {
        void AddOrUpdateJob(IJob job, string jobName, CancellationToken cancellationToken, TimeInterval interval = TimeInterval.Minute);

        void RemoveJob(string jobName);

        void StartJobs(CancellationToken cancellationToken);

        void StopAndRemoveAllJobs();
                
    }
}
