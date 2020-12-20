using Microsoft.Extensions.Configuration;
using Scheduler.Core.Entities;
using Scheduler.Core.Interfaces;
using Scheduler.Impl.CsvHelper;
using Scheduler.Impl.Logger;
using Scheduler.Impl.Mailer;
using Scheduler.Impl.WindowsService;
using System;
using System.IO;
using System.Threading;

namespace Scheduler.App
{
    public class Startup
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(global.AppSettingsFileName, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        static string _razorTemplate = Startup.Configuration.GetValue<string>(global.RazorTemplateFile);
        static string _mailDeliveryDirectory = FilePathFactory(Startup.Configuration.GetValue<string>(global.MailDeliveryDirectory));

        static ILogger _logger = new Logger(Startup.Configuration);
        static ICsvHelper _csvHelper = new Csv();
        static IMailer _mailer = new Mailer(_razorTemplate, _mailDeliveryDirectory);

        static string _customerDataFilePath = Startup.Configuration.GetValue<string>(global.CustomerDataFile);
        static string _interval = Startup.Configuration.GetValue<string>(global.MessageSendTimeInterval);
        static MailerJobSettings _jobSettings = MailerJobSettingsFactory(Startup.Configuration, _logger, _csvHelper, _mailer);

        

        public static IServiceSettings ServiceSettings { get; } = new SchedulerServiceSettings
        {
            InputFilePath = _customerDataFilePath,
            OutputFilePath = _mailDeliveryDirectory,

            TimeInterval = _interval,

            DataTemplate = _razorTemplate,

            Logger = _logger,
            CsvHelper = _csvHelper,
            Scheduler  = new Scheduler.Impl.Scheduler.Scheduler(_logger),
            Mailer = _mailer,

            //Job = new MailerJob(nameof(MailerJob), _interval, new System.Threading.CancellationToken(), _jobSettings)
            Job = new Scheduler.Impl.MediatorMailerJob.MailerJob(null, new CancellationToken(), _jobSettings)
        };

        public static string FilePathFactory(string fileOrDirectory)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileOrDirectory);

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

    }
}
