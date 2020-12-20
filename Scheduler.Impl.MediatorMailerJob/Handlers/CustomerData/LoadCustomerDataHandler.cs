using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.App.Entities;
using Scheduler.Impl.MediatorMailerJob.Adapters.CsvHelper;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.CustomerData
{
    public class LoadCustomerDataHandler : IRequestHandler<LoadCustomerData, IEnumerable<Customer>>
    {
        private readonly IMediator _mediator;

        public LoadCustomerDataHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<Customer>> Handle(LoadCustomerData request, CancellationToken cancellationToken)
        {
            var customerData = await request.GetCustomerDataAsync();

            await _mediator.Publish(new CustomerDataLoaded(customerData), cancellationToken);

            return customerData;
        }
    }
}
