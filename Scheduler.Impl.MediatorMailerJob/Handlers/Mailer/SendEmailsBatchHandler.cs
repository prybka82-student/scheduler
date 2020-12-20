using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Core.Entities;
using Scheduler.Impl.MediatorMailerJob.Adapters.Mailer;

namespace Scheduler.Impl.MediatorMailerJob.Handlers.Mailer
{
    public class SendEmailsBatchHandler : IRequestHandler<SendEmailsBatch, IEnumerable<(int id, Email email)>>
    {
        private readonly IMediator _mediator;

        public SendEmailsBatchHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<(int id, Email email)>> Handle(SendEmailsBatch request, CancellationToken cancellationToken)
        {
            var emailsBatch = await Task.Run(() => request.EmailsBatch);

            await _mediator.Publish(new EmailsBatchSent(emailsBatch), cancellationToken);

            return emailsBatch;
        }
    }
}
