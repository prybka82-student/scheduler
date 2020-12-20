using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Core.Interfaces
{
    public interface IScheduler
    {
        void AddJob(IJob job);

        Task StartAsync();

        void Cancel();
                
    }
}
