using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Impl.MediatorMailerJob.Adapters.CustomerDataToEmail;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.CustomerDataToEmail
{
    class CustomerDataToEmailConvertedHandler : INotificationHandler<CustomerDataToEmailConverted>
    {
        public async Task Handle(CustomerDataToEmailConverted notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"{DateTime.Now} - converted customer data to email");
        }
    }
}
