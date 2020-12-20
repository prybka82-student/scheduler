using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Scheduler.App.Entities;
using Scheduler.Core.Extensions;

namespace Scheduler.Impl.MediatorMailerJob.Adapters.CustomerDataToEmail
{
    public class ConvertCustomerDataToEmail : IRequest<IEnumerable<(int id, Email email)>>
    {
        private readonly IEnumerable<Customer> _customerDataBatch;

        private readonly Addressee _companyData;
        private readonly string _subjectTemplateDiscountPlaceholder;
        private readonly string _subjectTemplate;
        private readonly int _sentMessagesNumber;

        public ConvertCustomerDataToEmail(IEnumerable<Customer> customerDataBatch, 
            int sentMessagesNumber, 
            string subjectTemplate, string subjectTemplateDiscountPlaceholder,
            Addressee companyData)
        {
            _customerDataBatch = customerDataBatch;
            _sentMessagesNumber = sentMessagesNumber;
            _companyData = companyData;
            _subjectTemplate = subjectTemplate;
            _subjectTemplateDiscountPlaceholder = subjectTemplateDiscountPlaceholder;
        }

        public IEnumerable<(int id, Email email)> GetEmailsWithIds() => _customerDataBatch
            .CustomerDataToEmail(_sentMessagesNumber, _subjectTemplate, _subjectTemplateDiscountPlaceholder,
                _companyData);
    }
}
