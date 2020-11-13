using Microsoft.Extensions.Configuration;
using Scheduler.App.Entities;
using Scheduler.App.Extensions;
using Scheduler.App.Interfaces;
using System;
using System.Collections.Generic;
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
        private string _subjectTemplateDiscountPlaceholder = "#";

        public MailerJob(MailerJobSettings settings)
        {
            _customerDataFilePath = settings.CustomerDataFilePath;
            _batchSize = settings.BatchSize;
            _sentMessageIds = new List<int>();
            _mailer = settings.Mailer;
            _logger = settings.Logger;
            _csvHelper = settings.CsvHelper;
            _companyData = settings.CompanyData;
            _subjectTemplateDiscountPlaceholder = settings.SubjectTemplateDiscountPlaceholder;
            _subjectTemplate = settings.SubjectTemplate;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            var loadingFileResult = await _csvHelper.LoadFromFileAsync<Customer>(_customerDataFilePath, _sentMessageIds.Count, _batchSize);
            
            if (loadingFileResult.Result != ResultType.OK)
            {
                _logger.Warning($"Batch of client data from {_sentMessageIds.Count} to {_sentMessageIds.Count + _batchSize} was not retrieved");
                return;
            }

            var customerDataBatch = loadingFileResult.Data;

            if (customerDataBatch.IsNullOrEmpty())
            {
                _logger.Warning($"There are no client data records to process");
                return;
            }

            var emailsBatch = CustomerDataToEmail(customerDataBatch);

            if (emailsBatch.IsNullOrEmpty())
            {
                _logger.Warning($"There are no emails for sending");
                return;
            }

            await SendBatchAsync(emailsBatch, cancellationToken);            
        }

        private IEnumerable<(int id, Email email)> CustomerDataToEmail(IEnumerable<Customer> customerDataBatch)
        {
            var counter = _sentMessageIds.Count;

            foreach (var item in customerDataBatch)
            {
                var mail = new Email
                {
                    Content = item.Discount.ToString(),
                    Subject = _subjectTemplate.Replace(_subjectTemplateDiscountPlaceholder, item.Discount.ToString()),
                    From = _companyData,
                    To = new Addressee
                    {
                        Email = item.Email,
                        Name = item.Name,
                        Surname = item.Surname,
                        Title = item.Title
                    }
                };

                yield return (++counter, mail);
            }
        }

        private async Task SendMessageAsync(int emailId, Email email, CancellationToken cancellationToken)
        {
            if (_sentMessageIds.Contains(emailId).No())
            {
                _sentMessageIds.Add(emailId);
                await _mailer.SendAsync(email, cancellationToken, _logger);
            }
        }

        private async Task SendBatchAsync(IEnumerable<(int id, Email email)> emails, CancellationToken cancellationToken)
        {
            foreach (var email in emails)
            {
                await SendMessageAsync(email.id, email.email, cancellationToken);
            }
        }
                
    }
}
