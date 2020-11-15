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
using System.Threading;
using Topshelf;
using Scheduler.Impl.WindowsService;

namespace Scheduler.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //IDataGenerator<Customer> dataGenerator = new CustomerDataGenerator(123);
            //var customerData = dataGenerator.GenerateRecords(100_000, logger);
            //csvHelper.SaveToFile(customerData, customerDataFilePath, logger);

            /*
             * https://medium.com/wortell/building-a-windows-service-application-using-topshelf-part-1-of-2-getting-started-c76149e792ed
             */

            var serviceSettings = Startup.ServiceSettings;

            HostFactory.Run(hostConfig =>
            {
                hostConfig.Service<SchedulerService>(serviceConfig =>
                {
                    serviceConfig.ConstructUsing(() => new SchedulerService(serviceSettings));
                    serviceConfig.WhenStarted(async x => await x.StartAsync());
                    serviceConfig.WhenStopped(x => x.Stop());
                });

                hostConfig.RunAsLocalSystem();
                hostConfig.SetDescription("State-of-the-art mailing scheduler");
                hostConfig.SetDisplayName("Mailing Scheduler Service");
                hostConfig.SetServiceName("SchedulerService");
            });

            //testy

            var customerDataFilePath = Startup.Configuration.GetValue<string>(global.CustomerDataFile);
            var mailDeliveryDirectory = FilePathFactory(Startup.Configuration.GetValue<string>(global.MailDeliveryDirectory));
            var messageSendTimeInterval = Startup.Configuration.GetValue<string>(global.MessageSendTimeInterval);
            var messageTemplate = Startup.Configuration.GetValue<string>(global.MessageTemplate);
            var razorTemplate = Startup.Configuration.GetValue<string>(global.RazorTemplateFile);

            ILogger logger = new Logger(Startup.Configuration);
            ICsvHelper csvHelper = new Csv();
            IScheduler scheduler = new Scheduler.Impl.Scheduler.Scheduler(logger);
            IMailer mailer = new Mailer(messageTemplate, razorTemplate, mailDeliveryDirectory);

            var mailerJobSettings = MailerJobSettingsFactory(Startup.Configuration, logger, csvHelper, mailer);
            IJob job = new MailerJob(nameof(MailerJob), messageSendTimeInterval, new CancellationToken(), mailerJobSettings);

            IScheduler scheduler = Startup.ServiceSettings.Scheduler;
            IJob job = Startup.ServiceSettings.Job;

            scheduler.AddJob(job);

            await scheduler.StartAsync();
        }





    }
}
