using Microsoft.Extensions.Configuration;
using Scheduler.Core.Entities;
using Scheduler.Core.Interfaces;
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
        static void Main(string[] args)
        {
            //IDataGenerator<Customer> dataGenerator = new CustomerDataGenerator(123);
            //var customerData = dataGenerator.GenerateRecords(100_000, logger);
            //csvHelper.SaveToFile(customerData, customerDataFilePath, logger);

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

        }
    }
}
