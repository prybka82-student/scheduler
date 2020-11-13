using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.App.Interfaces
{
    public interface IJob
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}
