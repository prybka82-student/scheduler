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
using System.Threading.Tasks;

namespace Scheduler.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var customerDataFilePath = Startup.Configuration.GetValue<string>(global.CustomerDataFile);
            var mailDeliveryDirectory = FilePathFactory(Startup.Configuration.GetValue<string>(global.MailDeliveryDirectory));
            var messageSendTimeInterval = Startup.Configuration.GetValue<string>(global.MessageSendTimeInterval);
            var messageTemplate = Startup.Configuration.GetValue<string>(global.MessageTemplate);
            var sqlConnectionString = Startup.Configuration.GetValue<string>(global.SqlConnectionString);

            ILogger logger = new Logger(Startup.Configuration);
            ICsvHelper csvHelper = new Csv();
            IScheduler scheduler = new Scheduler.Impl.Scheduler.Scheduler(logger, sqlConnectionString);
            IMailer mailer = new Mailer(messageTemplate, mailDeliveryDirectory);

            //IDataGenerator<Customer> dataGenerator = new CustomerDataGenerator(123);
            //var customerData = dataGenerator.GenerateRecords(100_000, logger);
            //csvHelper.SaveToFile(customerData, customerDataFilePath, logger);

            var mailerJobSettings = MailerJobSettingsFactory(Startup.Configuration, logger, csvHelper, mailer);
            IJob job = new MailerJob(mailerJobSettings);

            var task = job.DoWorkAsync(new System.Threading.CancellationToken());

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
                CustomerDataFilePath = FilePathFactory(configuration.GetValue<string>(global.CustomerDataFile)),
                SubjectTemplate = configuration.GetValue<string>(global.SubjectTemplate),
                SubjectTemplateDiscountPlaceholder = configuration.GetValue<string>(global.SubjectTemplateDiscountPlaceholder)
            };
        }

        public static string FilePathFactory(string fileOrDirectory)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileOrDirectory);

    }
}
