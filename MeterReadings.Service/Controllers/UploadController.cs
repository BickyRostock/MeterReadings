using MeterReadings.Common.Data.Models;
using MeterReadings.Common.Services;
using MeterReadings.Service.Extensions;
using MeterReadings.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadings.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IReadingsRepository m_readingsRepo;

        public UploadController(IReadingsRepository readingsRepository)
        {
            m_readingsRepo = readingsRepository;
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<IActionResult> Upload([FromQuery] CsvFileDefinition csvFileDefinition, [FromForm] IFormFile file)
        {
            IActionResult result = Accepted();

            if (ValidateParameters(csvFileDefinition, file))
            {
                IEnumerable<IReading> readings = await ReadFileContents(file, csvFileDefinition);

                if (readings.Any())
                {
                    IEnumerable<IReading> invalidReadings = readings.Where(reading => !((Reading)reading).IsValid);
                    IEnumerable<IReading> validReadings = readings.Where(reading => ((Reading)reading).IsValid);

                    IEnumerable<IGrouping<(int AccountId, DateTime Date, int Value), IReading>> duplicates =
                        validReadings.GroupBy(reading => (reading.AccountId, reading.DateRecorded.Date, reading.Value));

                    IEnumerable<IReading> deduped = duplicates.Select(group => group.OrderByDescending(reading => reading.DateRecorded).FirstOrDefault());

                    _ = await m_readingsRepo.SaveReadingsAsync(deduped);

                    result = Accepted(new UploadResponse { InvalidReadings = invalidReadings, ValidReadings = deduped });
                }
                else
                {
                    result = BadRequest("File cannot be read.");
                }
            }
            else
            {
                result = BadRequest("Please enter valid column indexes that increment by 1, use file type 'CSV' and a file that contains data.");
            }

            return await Task.FromResult(result);
        }

        private static bool ValidateParameters(CsvFileDefinition definition, IFormFile file)
        {
            return definition.Validate() && file.Length > 0;
        }

        private static async Task<IEnumerable<IReading>> ReadFileContents(IFormFile file, CsvFileDefinition fileDefinition)
        {
            IList<IReading> readings = new List<IReading>();

            bool isFirstRow = fileDefinition.FileContainsHeaders;

            using (Stream stream = file.OpenReadStream())
            {
                if (stream.CanRead)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (reader.Peek() >= 0)
                        {
                            string line = await reader.ReadLineAsync();
                            if (!isFirstRow)
                            {
                                Reading reading = new Reading();
                                reading.ParseCsvString(line, fileDefinition.AccountIdColumnIndex, fileDefinition.DateRecordedColumnIndex, fileDefinition.ValueColumnIndex, fileDefinition.Delimiter);
                                reading.Validate();

                                readings.Add(reading);
                            }

                            isFirstRow = false;
                        }
                    }
                }
            }

            return readings;
        }
    }
}
