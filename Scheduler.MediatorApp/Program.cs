using System;
using System.Threading.Tasks;
using Scheduler.Core.Entities;
using Scheduler.Core.Interfaces;
using Scheduler.Impl.DataGenerator;

namespace Scheduler.MediatorApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceSettings = Startup.ServiceSettings;

            //generowanie danych
            //var logger = serviceSettings.Logger;
            //var csvHelper = serviceSettings.CsvHelper; 
            //var customerDataFilePath = serviceSettings.InputFilePath;

            //IDataGenerator<Customer> dataGenerator = new CustomerDataGenerator(123);
            //var customerData = dataGenerator.GenerateRecords(100_000, logger);
            //await csvHelper.SaveToFileAsync(customerData, customerDataFilePath, logger);

            var scheduler = serviceSettings.Scheduler;
            var job = serviceSettings.Job;
            scheduler.AddJob(job);
            await scheduler.StartAsync();

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.X)
                    scheduler.Cancel();
            }


        }
    }
}
