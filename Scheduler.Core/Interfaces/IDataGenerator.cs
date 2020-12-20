using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Interfaces
{
    public interface IDataGenerator<T> where T: class
    {
        IEnumerable<T> GenerateRecords(int howMany, ILogger logger = null);
    }
}
