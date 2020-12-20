using Microsoft.Extensions.Configuration;
using Scheduler.Core.Entities;
using Scheduler.Core.Extensions;
using Scheduler.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Impl.MailerJob
{
    public class MailerJob : IJob
    {
        private List<int> _sentMessageIds;
        private IMailer _mailer;
        private ILogger _logger;
        private ICsvHelper _csvHelper;
        private string _customerDataFilePath;
        private int _batchSize;
        private Addressee _companyData;
        private string _subjectTemplate;
        private readonly string _subjectTemplateDiscountPlaceholder = "#";

        public string Name { get; }

        public CancellationToken CancellationToken { get; set; }

        public string CronInterval { get; }

        public MailerJob(string name, string cronInterval, CancellationToken token, MailerJobSettings settings)
        {
            _sentMessageIds = new List<int>();
            
            CronInterval = cronInterval;
            CancellationToken = token;

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
                var customerDataBatch = await _csvHelper
                    .LoadFromFileAsync<Customer>(_customerDataFilePath, _sentMessageIds.Count, _batchSize);

                if (customerDataBatch == null) throw new ArgumentNullException();

                var emailsBatch = CustomerDataToEmail(customerDataBatch);

                if (emailsBatch == null) throw new NullReferenceException();

                await SendBatchAsync(emailsBatch, CancellationToken);
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

        private IEnumerable<(int id, Email email)> CustomerDataToEmail(IEnumerable<Customer> customerDataBatch)
            => customerDataBatch.CustomerDataToEmail(_sentMessageIds.Count, _subjectTemplate,
                _subjectTemplateDiscountPlaceholder, _companyData);

        private async Task SendMessageAsync(int emailId, Email email, CancellationToken token)
        {
            if (_sentMessageIds.Contains(emailId))
                return;

            _sentMessageIds
                .Add(emailId);
            
            await _mailer
                .SendAsync(email, token, _logger);
        }

        private async Task SendBatchAsync(IEnumerable<(int id, Email email)> emails, CancellationToken token)
        {
            foreach (var email in emails)
            {
                await SendMessageAsync(email.id, email.email, token);
            }
        }
    }
}
