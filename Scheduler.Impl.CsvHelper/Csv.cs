using Scheduler.App.Interfaces;
using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using Scheduler.App.Extensions;

namespace Scheduler.Impl.CsvHelper
{
    public class Csv : ICsvHelper
    {
        public async Task<List<T>> LoadFromFileAsync<T>(string path, int skip, int take, ILogger logger)
        {
            try
            {
                logger?.Debug($"Begininng to load data from {path}");

                var stopwatch = Stopwatch.StartNew();

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var result = csv.GetRecordsAsync<T>()
                    .Skip(skip)
                    .Take(take);

                var resultList = await result
                    .ToListAsync();

                logger?.Time(stopwatch.Elapsed, $"Successfully loaded {resultList.Count.ToString()} records from {path}");

                return resultList;
            }
            catch (FileNotFoundException e)
            {
                logger?.Exception(e, $"File {path} does not exist");
                throw e;
            }
            catch (Exception e)
            {
                logger?.Exception(e);
                throw e;
            }
        }

        public async Task SaveToFileAsync<T>(IEnumerable<T> data, string path, ILogger logger)
        {
            try
            {
                logger?.Debug($"Beginning to write data to path {path}");
                 
                var stopwatch = Stopwatch.StartNew();

                using var writer = new StreamWriter(path);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                await csv.WriteRecordsAsync(data);

                logger?.Time(stopwatch.Elapsed, $"Successfully write {data.Count()} records to {path}");
            }
            catch (DirectoryNotFoundException e)
            {
                logger?.Exception(e, $"Directory {Path.GetDirectoryName(path)} does not exist");
                throw e;
            }
            catch (Exception e)
            {
                logger?.Exception(e);
                throw e;
            }
        }        
    }
}
