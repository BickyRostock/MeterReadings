using MeterReadings.Common.Data.Models;
using System.Collections.Generic;

namespace MeterReadings.Service.Models
{
    public class UploadResponse
    {
        public IEnumerable<IReading> ValidReadings { get; set; }

        public IEnumerable<IReading> InvalidReadings { get; set; }
    }
}
