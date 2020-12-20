using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Impl.MediatorMailerJob.Adapters.CsvHelper;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.CustomerData
{
    public class CustomerDataLoadedHandler : INotificationHandler<CustomerDataLoaded>
    {
        public async Task Handle(CustomerDataLoaded notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"{DateTime.Now} - loaded {notification.CustomerData.Count()} customer data rows");
        }
    }
}
