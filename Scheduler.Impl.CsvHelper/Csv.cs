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
        public ActionResult<T> LoadFromFile<T>(string path, int skip = 0, int take = 0, ILogger logger = null)
        {
            var result = new T[0].Empty();

            try
            {
                logger?.Debug($"Begininng to load data from {path}");

                if (CheckIfFileExists(path).No()) throw new FileNotFoundException($"File {path} does not exist");

                var stopwatch = Stopwatch.StartNew();

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                if (take > 0)
                    result = csv.GetRecords<T>().Skip(skip).Take(take);
                else
                    result = csv.GetRecords<T>().Skip(skip);

                logger?.Time(stopwatch.Elapsed);

                logger?.Debug($"Successfully loaded {result.Count()} records from {path}");

                var res = new ActionResult<T>(ResultType.OK, result.ToList());

                return res;
            }
            catch (FileNotFoundException e)
            {
                logger?.Exception(e);
                throw e;
            }
            catch (Exception e)
            {
                logger?.Exception(e);
                throw e;
            }
        }

        public async Task<ActionResult<T>> LoadFromFileAsync<T>(string path, int skip = 0, int take = 0, ILogger logger = null)
        {
            var res = await Task.FromResult(LoadFromFile<T>(path, skip, take, logger));

            return res;
        }

        public ActionResult<T> SaveToFile<T>(IEnumerable<T> data, string path, ILogger logger = null)
        {
            try
            {
                logger?.Debug($"Beginning to write data to path {path}");

                if (CheckIfFolderExists(path)) throw new DirectoryNotFoundException($"Drive {Path.GetDirectoryName(path)} does not exist");

                var stopwatch = Stopwatch.StartNew();

                using var writer = new StreamWriter(path);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.WriteRecords(data);

                logger?.Time(stopwatch.Elapsed);

                return new ActionResult<T>(ResultType.OK, data.ToList());
            }
            catch (DirectoryNotFoundException e)
            {
                logger?.Exception(e);
                throw e;
            }
            catch (Exception e)
            {
                logger?.Exception(e);
                throw e;
            }
        }

        public async Task<ActionResult<T>> SaveToFileAsync<T>(IEnumerable<T> data, string path, ILogger logger = null)
            => await Task.FromResult(SaveToFile(data, path, logger));

        private bool CheckIfFileExists(string path) => File.Exists(path);

        private bool CheckIfFolderExists(string path) => Directory.Exists(Path.GetDirectoryName(path));

    }
}
