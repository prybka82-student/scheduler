using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.App.Interfaces
{
    public interface IMailer
    {           
        Task SendAsync(Email email, CancellationToken cancellation, ILogger logger);
    }
}
