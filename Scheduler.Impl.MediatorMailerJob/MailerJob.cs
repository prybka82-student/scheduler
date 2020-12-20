using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scheduler.App.Entities;
using Scheduler.App.Interfaces;
using Scheduler.Impl.MediatorMailerJob.Adapters;
using Scheduler.Impl.MediatorMailerJob.Adapters.CsvHelper;
using Scheduler.Impl.MediatorMailerJob.Adapters.CustomerDataToEmail;
using Scheduler.Impl.MediatorMailerJob.Adapters.Mailer;

namespace Scheduler.Impl.MediatorMailerJob
{
    public class MailerJob : IJob
    {
        private List<int> _sentMessageIds;
        private string _customerDataFilePath;
        private int _batchSize;
        private IMailer _mailer;
        private string _subjectTemplate;
        private string _subjectTemplateDiscountPlaceholder;
        private Addressee _companyData;
        private ILogger _logger;
        private ICsvHelper _csvHelper;
        private IMediator _mediator;
        public string Name { get; }
        public string CronInterval { get; } = "";
        public CancellationToken CancellationToken { get; set; }

        public MailerJob(IMediator mediator, CancellationToken token, MailerJobSettings settings)
        {
            _sentMessageIds = new List<int>();

            CancellationToken = token;

            _mediator = mediator;

            _customerDataFilePath = settings.CustomerDataFilePath;
            _batchSize = settings.BatchSize;
            _mailer = settings.Mailer;
            _logger = settings.Logger;
            _csvHelper = settings.CsvHelper;
            _companyData = settings.CompanyData;
            _subjectTemplateDiscountPlaceholder = settings.SubjectTemplateDiscountPlaceholder;
            _subjectTemplate = settings.SubjectTemplate;
        }

        public async Task DoWorkAsync()
        {
            try
            {
                var customerDataBatch =
                    await _mediator
                        .Send(new LoadCustomerData(_customerDataFilePath, _sentMessageIds.Count, _batchSize, _csvHelper, _logger));

                if (customerDataBatch == null) throw new ArgumentNullException("No customer data");

                var emailsBatch = await _mediator
                    .Send(new ConvertCustomerDataToEmail(customerDataBatch, _sentMessageIds.Count, _subjectTemplate, _subjectTemplateDiscountPlaceholder, _companyData));

                if (emailsBatch == null)
                    throw new NullReferenceException("Converting customer data to emails batch was unsuccessful");

                await _mediator
                    .Send(new SendEmailsBatch(emailsBatch));
            }
            catch (ArgumentNullException e)
            {
                _logger.Exception(e, "There is no customer data to process");
                throw e;
            }
            catch (NullReferenceException e)
            {
                _logger.Exception(e, "There are no emails to send");
                throw e;
            }
            catch (Exception e)
            {
                _logger.Exception(e);
                throw e;
            }
        }
    }
}
