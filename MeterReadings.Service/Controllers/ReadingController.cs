using MeterReadings.Common.Data.Models;
using MeterReadings.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reading = MeterReadings.Service.Models.Reading;

namespace MeterReadings.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingController : ControllerBase
    {
        private IReadingsRepository m_readingsRepo;

        public ReadingController(IReadingsRepository readingRepo)
        {
            m_readingsRepo = readingRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Reading>> GetListByAccountIdAsync(int accountId)
        {
            IEnumerable<IReading> readings = await m_readingsRepo.GetReadingsByAccountIdAsync(accountId);
            return readings.Select(r => new Reading(r));
        }

        [HttpGet("{id}")]
        public async Task<Reading> GetByIdAsync(int id)
        {
            IReading reading = await m_readingsRepo.GetReadingByIdAsync(id);

            return new Reading(reading);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] Reading reading)
        {
            _ = await m_readingsRepo.SaveReadingAsync(new ReadingEntity(reading));
        }

        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] Reading reading)
        {
            _ = await m_readingsRepo.SaveReadingAsync(new ReadingEntity(reading));
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            _ = await m_readingsRepo.DeleteReadingByIdAsync(id);
        }
    }
}
