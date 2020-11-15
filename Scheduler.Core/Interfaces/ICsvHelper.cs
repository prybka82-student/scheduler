using Scheduler.App.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.App.Interfaces
{
    public interface ICsvHelper
    {
        Task<List<T>> LoadFromFileAsync<T>(string path, int skip, int take, ILogger logger = null);
        Task SaveToFileAsync<T>(IEnumerable<T> data, string path, ILogger logger = null);
    }
}
