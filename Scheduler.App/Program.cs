using Microsoft.Extensions.Configuration;
using Scheduler.App.Entities;
using Scheduler.App.Interfaces;
using Scheduler.Impl.Logger;
using Scheduler.Impl.MailerJob;
using Scheduler.Impl.CsvHelper;
using Scheduler.Impl.DataGenerator;
using Scheduler.Impl.Mailer;
using System.IO;
using System;

namespace Scheduler.App
{
    class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(global.AppSettingsFileName, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        static void Main(string[] args)
        {
            var customerDataFilePath = Configuration.GetValue<string>(global.CustomerDataFile);
            var mailDeliveryDirectory = Configuration.GetValue<string>(global.MailDeliveryDirectory);
            var messageSendTimeInterval = Configuration.GetValue<string>(global.MessageSendTimeInterval);
            var messageTemplate = Configuration.GetValue<string>(global.MessageTemplate);

            ILogger logger = new Logger(Configuration);
            ICsvHelper csvHelper = new Csv();
            IScheduler scheduler = new Scheduler.Impl.Scheduler.Scheduler(logger);
            IMailer mailer = new Mailer(messageTemplate, mailDeliveryDirectory);

            IDataGenerator<Customer> dataGenerator = new CustomerDataGenerator(123);
            var customerData = dataGenerator.GenerateRecords(100_000, logger);
            csvHelper.SaveToFile(customerData, customerDataFilePath, logger);

            var mailerJobSettings = MailerJobSettingsFactory(Configuration, logger, csvHelper, mailer);
            IJob job = new MailerJob(mailerJobSettings);

            scheduler.AddOrUpdateJob(job, "mailerJob", new System.Threading.CancellationToken(), messageSendTimeInterval);

            scheduler.StartJobs(new System.Threading.CancellationToken());

        }

        public static MailerJobSettings MailerJobSettingsFactory(IConfiguration configuration, ILogger logger, ICsvHelper csvHelper, IMailer mailer)
        {
            var companyData = new Addressee();
            configuration.Bind(global.CompanyData, companyData);

            return new MailerJobSettings
            {
                Logger = logger,
                CsvHelper = csvHelper,
                Mailer = mailer,

                BatchSize = configuration.GetValue<int>(global.BatchSize),
                CompanyData = companyData,
                CustomerDataFilePath = configuration.GetValue<string>(global.CustomerDataFile),
                SubjectTemplate = configuration.GetValue<string>(global.SubjectTemplate),
                SubjectTemplateDiscountPlaceholder = configuration.GetValue<string>(global.SubjectTemplateDiscountPlaceholder)
            };
        }

    }
}
