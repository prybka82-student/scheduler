using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.App
{
    class global
    {
        public static readonly string AppSettingsFileName = "appsettings.json";
        public static readonly string CustomerDataFile = "CustomerDataFile";
        public static readonly string MailDeliveryDirectory = "MailDeliveryDirectory";
        public static readonly string BatchSize = "BatchSize";
        public static readonly string MessageSendTimeInterval = "MessageSendTimeInterval";
        public static readonly string CompanyData = "CompanyData";
        public static readonly string MessageTemplate = "MessageTemplate";
        public static readonly string SubjectTemplate = "SubjectTemplate";
        public static readonly string SubjectTemplateDiscountPlaceholder = "SubjectTemplateDiscountPlaceholder";
        public static readonly string SqlConnectionString = "SqlConnectionString";
        internal static readonly string RazorTemplateFile = "RazorTemplateFile";
    }
}
