using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Impl.MediatorMailerJob.Adapters.Mailer;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.Mailer
{
    public class EmailsBatchSentHandler : INotificationHandler<EmailsBatchSent>
    {
        public async Task Handle(EmailsBatchSent notification, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync($"{DateTime.Now} - emails sent");
        }
    }
}
