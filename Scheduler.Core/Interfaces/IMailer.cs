using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scheduler.Core.Entities;

namespace Scheduler.Core.Interfaces
{
    public interface IMailer
    {           
        Task SendAsync(Email email, CancellationToken cancellation, ILogger logger);
    }
}
