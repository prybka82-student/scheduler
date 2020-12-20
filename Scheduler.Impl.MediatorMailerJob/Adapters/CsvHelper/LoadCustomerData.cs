using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Scheduler.Core.Entities;
using Scheduler.Core.Interfaces;

namespace Scheduler.Impl.MediatorMailerJob.Adapters.CsvHelper
{
    public class LoadCustomerData : IRequest<IEnumerable<Customer>>
    {
        private readonly ICsvHelper _csvHelper;
        private readonly string _customerDataFilePath;
        private readonly int _skip;
        private readonly int _take;
        private readonly ILogger _logger;

        public LoadCustomerData(string customerDataFilePath, in int skip, in int batchSize, ICsvHelper csvHelper, ILogger logger = null)
        {
            _csvHelper = csvHelper;
            _customerDataFilePath = customerDataFilePath;
            _skip = skip;
            _take = batchSize;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetCustomerDataAsync() =>
            await _csvHelper
                .LoadFromFileAsync<Customer>(_customerDataFilePath, _skip, _take, _logger);
    }
}
