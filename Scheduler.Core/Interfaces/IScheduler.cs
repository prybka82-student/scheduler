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
        void AddJob(IJob job);

        Task StartAsync();
                
    }
}
