using System;
using System.Collections.Generic;
using System.Text;
using Scheduler.Core.Interfaces;

namespace Scheduler.Core.Entities
{
    public class MailerJobSettings
    {
        public string CustomerDataFilePath { get; set; }
        public int BatchSize { get; set; }
        public IMailer Mailer { get; set; }
        public ILogger Logger { get; set; }
        public ICsvHelper CsvHelper { get; set; }
        public string SubjectTemplate { get; set; }
        public string SubjectTemplateDiscountPlaceholder { get; set; }
        public Addressee CompanyData { get; set; }
    }
}
