using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Scheduler.Core.Entities;

namespace Scheduler.Impl.MediatorMailerJob.Adapters.CsvHelper
{
    public class CustomerDataLoaded : INotification
    {
        public IEnumerable<Customer> CustomerData { get; }

        public CustomerDataLoaded(IEnumerable<Customer> customerData)
        {
            CustomerData = customerData;
        }
    }
}
