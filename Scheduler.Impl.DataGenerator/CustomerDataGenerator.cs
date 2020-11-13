using Bogus;
using Scheduler.App.Entities;
using Scheduler.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scheduler.Impl.DataGenerator
{
    public class CustomerDataGenerator : IDataGenerator<Customer>
    {
        public CustomerDataGenerator(int randomSeed)
        {
            Randomizer.Seed = new Random(randomSeed);
        }

        public IEnumerable<Customer> GenerateRecords(int howMany, ILogger logger = null)
        {
            try
            {
                var personId = 0;

                logger?.Debug();

                var stopwatch = Stopwatch.StartNew();

                var res = new Faker<Customer>()
                   .RuleFor(u => u.Id, (f, u) => personId++)
                   .RuleFor(u => u.Name, (f, u) => f.Name.FindName())
                   .RuleFor(u => u.Surname, (f, u) => f.Name.LastName())
                   .RuleFor(u => u.Title, (f, u) => f.Name.Prefix())
                   .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name, u.Surname))
                   .RuleFor(u => u.Discount, (f, u) => Math.Round(f.Random.Decimal(0.1M, 0.8M), 2))
                   .Generate(howMany);

                logger?.Time(stopwatch.Elapsed);

                return res;
            }
            catch (Exception e)
            {
                logger?.Exception(e);
                throw e;
            }
        }
    }
}
