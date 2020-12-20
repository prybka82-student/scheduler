using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Scheduler.App.Entities;

namespace Scheduler.Impl.MediatorMailerJob.Adapters.Mailer
{
    public class EmailsBatchSent : INotification
    {
        public IEnumerable<(int id, Email email)> EmailsBatch { get; }

        public EmailsBatchSent(IEnumerable<(int id, Email email)> emailsBatch)
        {
            EmailsBatch = emailsBatch;
        }
    }
}
