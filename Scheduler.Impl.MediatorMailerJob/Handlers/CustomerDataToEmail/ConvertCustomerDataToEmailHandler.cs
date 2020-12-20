using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Core.Entities;
using Scheduler.Impl.MediatorMailerJob.Adapters.CustomerDataToEmail;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.CustomerDataToEmail
{
    public class ConvertCustomerDataToEmailHandler : IRequestHandler<ConvertCustomerDataToEmail, IEnumerable<(int id, Email email)>>
    {
        private readonly IMediator _mediator;

        public ConvertCustomerDataToEmailHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<(int id, Email email)>> Handle(ConvertCustomerDataToEmail request,
            CancellationToken cancellationToken)
        {
            var emailsWithIds = await Task.Run(() => request.GetEmailsWithIds());

            await _mediator.Publish(new CustomerDataToEmailConverted(emailsWithIds), cancellationToken);

            return emailsWithIds;
        }
    }
}
