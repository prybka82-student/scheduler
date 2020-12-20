using System;
using System.Collections.Generic;
using System.Text;
using Scheduler.Core.Entities;

namespace Scheduler.Core.Extensions
{
    public static class CustomerExtensions
    {
        public static IEnumerable<(int id, Email email)> CustomerDataToEmail(
            this IEnumerable<Customer> customerDataBatch, 
            int sentMessagesNumber, string subjectTemplate, string subjectTemplateDiscountPlaceholder, Addressee companyData)
        {
            var counter = sentMessagesNumber;

            foreach (var item in customerDataBatch)
            {
                var discount = item
                    .Discount
                    .ToString()
                    .ToPercent();

                var addressee = new Addressee
                {
                    Email = item.Email,
                    Name = item.Name,
                    Surname = item.Surname,
                    Title = item.Title
                };

                var mail = new Email
                {
                    Content = discount,
                    Subject = subjectTemplate
                        .Replace(subjectTemplateDiscountPlaceholder, discount),
                    From = companyData,
                    To = addressee
                };

                yield return (++counter, mail);
            }
        }
    }
}
