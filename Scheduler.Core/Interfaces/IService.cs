using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Interfaces
{
    public interface IService
    {
        Task StartAsync();
        void Stop();
    }
}
