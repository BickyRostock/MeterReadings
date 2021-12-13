using MeterReadings.Common.Data;
using MeterReadings.Common.Data.Models;
using MeterReadings.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadings.Common.Services
{
    public class ReadingsRepository : IReadingsRepository
    {
        private CommonDBContext m_dbContext;

        public ReadingsRepository(CommonDBContext context)
        {
            m_dbContext = context;
        }

        public async Task<int> DeleteReadingByIdAsync(int id)
        {
            IReading reading = await m_dbContext.Readings.FindAsync(id);

            m_dbContext.Remove(reading);

            return await m_dbContext.SaveChangesAsync();
        }

        public async Task<IReading> GetReadingByIdAsync(int id)
        {
            return await m_dbContext.Readings.FindAsync(id);
        }

        public Task<IEnumerable<IReading>> GetReadingsByAccountIdAsync(int accountId)
        {
            IEnumerable<IReading> accountReadings = m_dbContext.Readings.Where(r => r.AccountId == accountId);

            return Task.FromResult(accountReadings);
        }

        public async Task<IReading> SaveReadingAsync(IReading reading)
        {
            IReading existingReading = await CheckIfEntryAlreadyExists(reading);

            if(existingReading is not null)
            {
                existingReading.DateRecorded = reading.DateRecorded;
                existingReading.Value = reading.Value;
                reading = existingReading;
            }
            else
            {
                reading = new ReadingEntity(reading);
                await m_dbContext.AddAsync(reading);
            }

            _ = await m_dbContext.SaveChangesAsync();

            return reading;
        }

        public async Task<IEnumerable<IReading>> SaveReadingsAsync(IEnumerable<IReading> readings)
        {
            IEnumerable<IGrouping<int, IReading>> accountGroups = readings.GroupBy(reading => reading.AccountId);

            IEnumerable<IReading> updatedReadings = new List<IReading>();
            IEnumerable<IReading> newReadings = new List<IReading>();

            foreach(IGrouping<int, IReading> accountGroup in accountGroups)
            {
                IEnumerable<IReading> existingAccountReadings = await GetReadingsByAccountIdAsync(accountGroup.FirstOrDefault().AccountId);

                foreach(IReading readingToCheck in accountGroup)
                {
                    IReading existingReading = existingAccountReadings.Where(reading => reading.Compare(readingToCheck)).FirstOrDefault();

                    if(existingReading is not null)
                    {
                        existingReading.DateRecorded = readingToCheck.DateRecorded;
                        existingReading.Value = readingToCheck.Value;
                        updatedReadings = updatedReadings.Append(existingReading);
                    }
                    else
                    {
                        updatedReadings = updatedReadings.Append(readingToCheck);
                        newReadings = newReadings.Append(new ReadingEntity(readingToCheck));
                    }
                }
            }



            //IEnumerable<ReadingEntity> readingsToSave = readings.Select(reading => new ReadingEntity(reading));

            await m_dbContext.AddRangeAsync(newReadings);

            _ = await m_dbContext.SaveChangesAsync();

            return updatedReadings;
        }

        private async Task<IReading> CheckIfEntryAlreadyExists(IReading readingToCheck)
        {
            IEnumerable<IReading> accountReadings = await GetReadingsByAccountIdAsync(readingToCheck.AccountId);

            IReading sameEntry = accountReadings.Where(reading => reading.Compare(readingToCheck)).FirstOrDefault();
            
            return sameEntry;
        }
    }
}
