using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.App.Interfaces
{
    public interface ICsvHelper
    {
        ActionResult<T> LoadFromFile<T>(string path, int skip = 0, int take = 0, ILogger logger = null);
        Task<ActionResult<T>> LoadFromFileAsync<T>(string path, int skip = 0, int take = 0, ILogger logger = null);

        ActionResult<T> SaveToFile<T>(IEnumerable<T> data, string path, ILogger logger = null);
        Task<ActionResult<T>> SaveToFileAsync<T>(IEnumerable<T> data, string path, ILogger logger = null);
    }
}
