using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Core.Interfaces
{
    public interface IJob
    {
        string Name { get; }

        string CronInterval { get; }

        CancellationToken CancellationToken { get; set; }

        Task DoWorkAsync();
    }
}
