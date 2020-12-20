using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.App.Entities;

namespace Scheduler.Impl.MediatorMailerJob.Adapters.CustomerDataToEmail
{
    public class CustomerDataToEmailConverted : INotification
    {
        public IEnumerable<(int id, Email email)> EmailsWithIds { get; }

        public CustomerDataToEmailConverted(IEnumerable<(int id, Email email)> emailsWithIds)
        {
            EmailsWithIds = emailsWithIds;
        }
    }
}
