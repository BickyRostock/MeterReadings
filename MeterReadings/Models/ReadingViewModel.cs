using MeterReadings.Common.Data.Models;
using System;

namespace MeterReadings.Models
{
    public class ReadingViewModel : IReading
    {
        public DateTime DateRecorded { get; set; }
        public int Id { get; set; }
        public int Value { get; set; }
        public int AccountId { get; set; }
    }
}
