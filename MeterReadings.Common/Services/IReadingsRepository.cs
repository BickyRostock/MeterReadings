using MeterReadings.Common.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReadings.Common.Services
{
    public interface IReadingsRepository
    {
        public Task<IReading> SaveReadingAsync(IReading reading);

        public Task<IEnumerable<IReading>> SaveReadingsAsync(IEnumerable<IReading> readings);

        public Task<IReading> GetReadingByIdAsync(int id);

        public Task<IEnumerable<IReading>> GetReadingsByAccountIdAsync(int accountId);

        public Task<int> DeleteReadingByIdAsync(int id);
    }
}
